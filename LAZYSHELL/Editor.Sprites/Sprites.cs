using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using LAZYSHELL.Properties;

namespace LAZYSHELL
{
    public partial class Sprites : Form
    {
        #region Variables

        private long checksum;
        public long Checksum { get { return checksum; } set { checksum = value; } }
        // main
        private delegate void Function();
        private byte[] data;
        private Settings settings = Settings.Default;
        private Overlay overlay;
        private bool updating = false;
        private Sprite[] sprites;
        private Animation[] animations;
        private PaletteSet[] palettes;
        private GraphicPalette[] images;
        private int availableBytes = 0;
        private byte[] graphics;
        public byte[] Graphics
        {
            get { return graphics; }
            set
            {
                graphics = value;
                graphics.CopyTo(Model.SpriteGraphics, image.GraphicOffset - 0x280000);
            }
        }
        private byte[] spriteGraphics { get { return Model.SpriteGraphics; } }
        // indexed variables
        public int Index { get { return (int)number.Value; } set { number.Value = value; } }
        private Sprite sprite { get { return sprites[Index]; } set { sprites[Index] = value; } }
        private GraphicPalette image { get { return images[sprite.GraphicPalettePacket]; } set { images[sprite.GraphicPalettePacket] = value; } }
        private Animation animation { get { return animations[sprite.AnimationPacket]; } set { animations[sprite.AnimationPacket] = value; } }
        private PaletteSet paletteSet { get { return palettes[image.PaletteNum + sprite.PaletteIndex]; } set { palettes[image.PaletteNum + sprite.PaletteIndex] = value; } }
        // public variables
        public Sprite Sprite { get { return sprite; } set { sprite = value; } }
        public GraphicPalette Image { get { return image; } set { image = value; } }
        public Animation Animation { get { return animation; } set { animation = value; } }
        public int[] Palette { get { return paletteSet.Palette; } }
        public PaletteSet PaletteSet { get { return paletteSet; } set { paletteSet = value; } }
        public int AvailableBytes { get { return availableBytes; } set { availableBytes = value; } }
        // editors
        private SpriteMolds molds;
        public SpriteMolds Molds { get { return molds; } set { molds = value; } }
        private SpriteSequences sequences;
        public SpriteSequences Sequences { get { return sequences; } set { sequences = value; } }
        private PaletteEditor paletteEditor;
        private GraphicEditor graphicEditor;
        private Search searchWindow;
        // special controls
        #endregion
        #region Methods
        // main
        public Sprites()
        {
            InitializeComponent();
            Do.AddShortcut(toolStrip3, Keys.Control | Keys.S, new EventHandler(save_Click));
            Do.AddShortcut(toolStrip3, Keys.F1, helpTips);
            Do.AddShortcut(toolStrip3, Keys.F2, baseConvertor);
            toolTip1.InitialDelay = 0;
            searchWindow = new Search(number, nameTextBox, searchEffectNames, name.Items);
            // set data
            this.data = Model.Data;
            this.sprites = Model.Sprites;
            this.animations = Model.Animations;
            this.palettes = Model.SpritePalettes;
            this.images = Model.GraphicPalettes;
            this.overlay = new Overlay();
            graphics = image.Graphics(spriteGraphics);
            // controls
            updating = true;
            name.Items.AddRange(Lists.Numerize(Lists.SpriteNames));
            if (settings.RememberLastIndex)
            {
                name.SelectedIndex = settings.LastSprite;
                number.Value = settings.LastSprite;
            }
            else
                name.SelectedIndex = 0;
            foreach (Animation a in animations)
                a.Assemble();
            RefreshSpritesEditor();
            updating = false;
            GC.Collect();
            // editors
            molds.TopLevel = false;
            molds.Dock = DockStyle.Fill;
            panelMolds.Controls.Add(molds);
            molds.BringToFront();
            openMolds.Checked = true;
            molds.Visible = true;
            sequences.TopLevel = false;
            sequences.Dock = DockStyle.Fill;
            panelSequences.Controls.Add(sequences);
            sequences.SendToBack();
            openSequences.Checked = true;
            sequences.Visible = true;
            new ToolTipLabel(this, baseConvertor, helpTips);
            //
            new History(this);
            checksum = Do.GenerateChecksum(sprites, animations, images, palettes, graphics);
        }
        private void RefreshSpritesEditor()
        {
            Cursor.Current = Cursors.WaitCursor;
            updating = true;
            paletteIndex.Value = sprite.PaletteIndex;
            imageNum.Value = sprite.GraphicPalettePacket;
            paletteOffset.Value = image.PaletteNum;
            graphicOffset.Value = image.GraphicOffset;
            graphics = image.Graphics(spriteGraphics);
            animationPacket.Value = sprite.AnimationPacket;
            animationVRAM.Value = animation.VramAllocation;
            LoadMoldEditor();
            LoadSequenceEditor();
            LoadPaletteEditor();
            LoadGraphicEditor();
            CalculateFreeSpace();
            updating = false;
            GC.Collect();
            Cursor.Current = Cursors.Arrow;
        }
        public void CalculateFreeSpace()
        {
            int totalSize, min, max;
            int length = 0;

            if (sprite.AnimationPacket < 42)
            {
                totalSize = 0x7000; min = 0; max = 42;
            }
            else if (sprite.AnimationPacket < 107)
            {
                totalSize = 0xFFFF; min = 42; max = 107;
            }
            else if (sprite.AnimationPacket < 249)
            {
                totalSize = 0xFFFF; min = 107; max = 249;
            }
            else
            {
                totalSize = 0xFFFF; min = 249; max = 444;
            }
            for (int i = min; i < max; i++)
                length += animations[i].SM.Length;
            availableBytes = totalSize - length;
            animationAvailableBytes.BackColor = availableBytes > 0 ? Color.Lime : Color.Red;
            animationAvailableBytes.Text = availableBytes.ToString() + " bytes free (animations)";
        }
        public void Assemble()
        {
            ProgressBar progressBar = new ProgressBar("ASSEMBLING ANIMATIONS...", animations.Length);
            progressBar.Show();
            int i = 0;
            foreach (Animation sm in animations)
            {
                sm.Assemble();
                progressBar.PerformStep("ASSEMBLING ANIMATION #" + i);
                i++;
            }
            progressBar.Close();
            i = 0;
            int pointer = 0x252000;
            int offset = 0x259000;
            for (; i < 42 && offset < 0x25FFFF; i++, pointer += 3)
            {
                if (animations[i].SM.Length + offset > 0x25FFFF)
                    break;
                Bits.SetShort(data, pointer, (ushort)offset);
                Bits.SetByte(data, pointer + 2, (byte)((offset >> 16) + 0xC0));
                Bits.SetByteArray(data, offset, animations[i].SM);
                offset += animations[i].SM.Length;
            }
            if (i < 42)
                MessageBox.Show("The available space for animation data in bank 0x250000 has exceeded the alotted space.\nAnimation #'s " + i.ToString() + " through 41 will not saved. Please make sure the available animation bytes is not negative.", "LAZY SHELL");
            offset = 0x260000;
            for (; i < 107 && offset < 0x26FFFF; i++, pointer += 3)
            {
                if (animations[i].SM.Length + offset > 0x26FFFF)
                    break;
                Bits.SetShort(data, pointer, (ushort)offset);
                Bits.SetByte(data, pointer + 2, (byte)((offset >> 16) + 0xC0));
                Bits.SetByteArray(data, offset, animations[i].SM);
                offset += animations[i].SM.Length;
            }
            if (i < 107)
                MessageBox.Show("The available space for animation data in bank 0x260000 has exceeded the alotted space.\nAnimation #'s " + i.ToString() + " through 107 will not saved. Please make sure the available animation bytes is not negative.", "LAZY SHELL");
            offset = 0x270000;
            for (; i < 249 && offset < 0x27FFFF; i++, pointer += 3)
            {
                if (animations[i].SM.Length + offset > 0x27FFFF)
                    break;
                Bits.SetShort(data, pointer, (ushort)offset);
                Bits.SetByte(data, pointer + 2, (byte)((offset >> 16) + 0xC0));
                Bits.SetByteArray(data, offset, animations[i].SM);
                offset += animations[i].SM.Length;
            }
            if (i < 249)
                MessageBox.Show("The available space for animation data in bank 0x270000 has exceeded the alotted space.\nAnimation #'s " + i.ToString() + " through 249 will not saved. Please make sure the available animation bytes is not negative.", "LAZY SHELL");
            offset = 0x360000;
            for (; i < 444 && offset < 0x36FFFF; i++, pointer += 3)
            {
                if (animations[i].SM.Length + offset > 0x36FFFF)
                    break;
                Bits.SetShort(data, pointer, (ushort)offset);
                Bits.SetByte(data, pointer + 2, (byte)((offset >> 16) + 0xC0));
                Bits.SetByteArray(data, offset, animations[i].SM);
                offset += animations[i].SM.Length;
            }
            if (i < 444)
                MessageBox.Show("The available space for animation data in bank 0x360000 has exceeded the alotted space.\nAnimation #'s " + i.ToString() + " through 444 will not saved. Please make sure the available animation bytes is not negative.", "LAZY SHELL");

            foreach (Sprite s in sprites)
                s.Assemble();
            foreach (GraphicPalette gp in images)
                gp.Assemble();
            foreach (PaletteSet p in palettes)
                p.Assemble(0);
            Buffer.BlockCopy(Model.SpriteGraphics, 0, data, 0x280000, 0xB4000);

            Model.HexViewer.Offset = animation.AnimationOffset & 0xFFFFF0;
            Model.HexViewer.SelectionStart = (animation.AnimationOffset & 15) * 3;
            Model.HexViewer.Compare();
            checksum = Do.GenerateChecksum(sprites, animations, images, palettes, graphics);
            Do.AddHistory(this, Index, "SaveSprites");
        }
        public void EnableOnPlayback(bool enable)
        {
            foreach (Control control in this.Controls)
                if (control != panelSequences)
                    control.Enabled = enable;
                else
                    foreach (Control parent in panelSequences.Controls)
                        if (parent != sequences)
                            parent.Enabled = enable;
                        else
                            foreach (Control child in parent.Controls)
                                if (child.Name != "toolStrip1")
                                    child.Enabled = enable;
                                else
                                    foreach (ToolStripItem item in ((ToolStrip)child).Items)
                                        if (item.Name != "pause")
                                            item.Enabled = enable;
        }
        // tooltips
        // editors
        public void LoadPaletteEditor()
        {
            if (paletteEditor == null)
            {
                paletteEditor = new PaletteEditor(new Function(PaletteUpdate), paletteSet, 1, 0,1);
                paletteEditor.FormClosing += new FormClosingEventHandler(editor_FormClosing);
            }
            else
                paletteEditor.Reload(new Function(PaletteUpdate), paletteSet, 1, 0,1);
        }
        public void LoadGraphicEditor()
        {
            if (graphicEditor == null)
            {
                graphicEditor = new GraphicEditor(new Function(GraphicUpdate),
                    graphics, graphics.Length, 0, paletteSet, 0, 0x20, 1);
                graphicEditor.FormClosing += new FormClosingEventHandler(editor_FormClosing);
            }
            else
                graphicEditor.Reload(new Function(GraphicUpdate),
                    graphics, graphics.Length, 0, paletteSet, 0, 0x20, 1);
        }
        private void LoadMoldEditor()
        {
            if (molds == null)
                molds = new SpriteMolds(this);
            else
                molds.Reload(this);
        }
        private void LoadSequenceEditor()
        {
            if (sequences == null)
                sequences = new SpriteSequences(this);
            else
                sequences.Reload(this);
        }
        public void PaletteUpdate()
        {
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            sequences.InvalidateImages();
            LoadGraphicEditor();
            checksum--;   // b/c switching colors won't modify checksum
        }
        public void GraphicUpdate()
        {
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            sequences.InvalidateImages();
            graphics.CopyTo(Model.SpriteGraphics, image.GraphicOffset - 0x280000);
        }
        private void CloseEditors()
        {
            graphicEditor.Close();
            paletteEditor.Close();
            searchWindow.Close();
            graphicEditor.Dispose();
            paletteEditor.Dispose();
            searchWindow.Dispose();
        }
        #endregion
        #region Event Handlers
        // main
        private void Sprites_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Do.GenerateChecksum(sprites, animations, images, palettes, graphics) == checksum)
                goto Close;
            DialogResult result = MessageBox.Show(
                "Sprites have not been saved.\n\nWould you like to save changes?", "LAZY SHELL",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                Assemble();
            else if (result == DialogResult.No)
            {
                CloseEditors();
                Model.Sprites = null;
                Model.SpriteGraphics = null;
                Model.SpritePalettes = null;
                Model.Animations = null;
                Model.GraphicPalettes = null;
                return;
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
        Close:
            CloseEditors();
        }
        private void number_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            name.SelectedIndex = (int)number.Value;
            animation.Assemble();
            RefreshSpritesEditor();
            settings.LastSprite = Index;
        }
        private void name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating) return;
            number.Value = name.SelectedIndex;
        }
        private void paletteIndex_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            sprite.PaletteIndex = (byte)paletteIndex.Value;
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            LoadPaletteEditor();
            LoadGraphicEditor();
        }
        private void imageNum_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            sprite.GraphicPalettePacket = (ushort)imageNum.Value;
            paletteOffset.Value = image.PaletteNum;
            graphicOffset.Value = image.GraphicOffset;
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            LoadPaletteEditor();
            LoadGraphicEditor();
        }
        private void paletteOffset_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            image.PaletteNum = (int)paletteOffset.Value;
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            LoadPaletteEditor();
            LoadGraphicEditor();
        }
        private void graphicOffset_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            graphicOffset.Value = (int)graphicOffset.Value & 0xFFFFE0;
            image.GraphicOffset = (int)graphicOffset.Value;
            graphics = image.Graphics(spriteGraphics);
            foreach (Mold mold in animation.Molds)
            {
                foreach (Mold.Tile tile in mold.Tiles)
                    tile.Set8x8Tiles(graphics, paletteSet.Palette, tile.Gridplane);
            }
            molds.SetTilesetImage();
            molds.SetTilemapImage();
            sequences.SetSequenceFrameImages();
            LoadGraphicEditor();
        }
        private void animationPacket_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            sprite.AnimationPacket = (ushort)animationPacket.Value;
            animationVRAM.Value = animation.VramAllocation;
            LoadMoldEditor();
            LoadSequenceEditor();
        }
        private void animationVRAM_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            animation.VramAllocation = (ushort)animationVRAM.Value;
            molds.SetAvailableVRAM();
        }
        // editors
        private void showMain_Click(object sender, EventArgs e)
        {
            panel1.Visible = showMain.Checked;
        }
        private void openPalettes_Click(object sender, EventArgs e)
        {
            paletteEditor.Visible = true;
        }
        private void openGraphics_Click(object sender, EventArgs e)
        {
            graphicEditor.Visible = true;
        }
        private void openSequences_Click(object sender, EventArgs e)
        {
            sequences.Visible = openSequences.Checked;
        }
        private void openMolds_Click(object sender, EventArgs e)
        {
            molds.Visible = openMolds.Checked;
        }
        private void editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ((Form)sender).Hide();
        }
        // data managing
        private void save_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Assemble();
            Cursor.Current = Cursors.Arrow;
        }
        private void import_Click(object sender, EventArgs e)
        {
            IOElements ioelements = new IOElements(animations, (int)animationPacket.Value, "IMPORT SPRITE ANIMATIONS...");
            if (ioelements.ShowDialog() == DialogResult.Cancel)
                return;
            foreach (Animation sm in animations)
                sm.Assemble();
            RefreshSpritesEditor();
        }
        private void export_Click(object sender, EventArgs e)
        {
            new IOElements(animations, (int)animationPacket.Value, "EXPORT SPRITE ANIMATIONS...").ShowDialog();
        }
        private void clear_Click(object sender, EventArgs e)
        {
            ClearElements clearElements = new ClearElements(animations, (int)animationPacket.Value, "CLEAR SPRITE ANIMATIONS...");
            clearElements.ShowDialog();
            if (clearElements.DialogResult == DialogResult.Cancel)
                return;
            foreach (Animation sm in animations)
                sm.Assemble();
            RefreshSpritesEditor();
        }
        private void reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You're about to undo all changes to the current sprite and animation index. Go ahead with reset?",
                "LAZY SHELL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            animation = new Animation(Model.Data, sprite.AnimationPacket);
            image = new GraphicPalette(Model.Data, sprite.GraphicPalettePacket);
            sprite = new Sprite(Model.Data, Index);
            for (int i = image.PaletteNum; i < image.PaletteNum + 8; i++)
                palettes[i] = new PaletteSet(Model.Data, i, 0x252FFE + (i * 30), 1, 16, 30);
            Buffer.BlockCopy(Model.Data, image.GraphicOffset, Model.SpriteGraphics, image.GraphicOffset - 0x280000, 0x4000);
            number_ValueChanged(null, null);
        }
        private void hexViewer_Click(object sender, EventArgs e)
        {
            Model.HexViewer.Offset = animation.AnimationOffset & 0xFFFFF0;
            Model.HexViewer.SelectionStart = (animation.AnimationOffset & 15) * 3;
            Model.HexViewer.Compare();
            Model.HexViewer.Show();
        }
        private void allMoldImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportImages exportImages = new ExportImages(Index, "sprites");
            exportImages.ShowDialog();
        }
        private void allSequenceImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}