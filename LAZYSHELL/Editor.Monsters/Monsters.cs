﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LAZYSHELL.Properties;

namespace LAZYSHELL
{
    public partial class Monsters : Form
    {
        #region Variables
        
        private delegate void Function();
        private long checksum;
        private bool updatingMonsters = false;
        private Monster[] monsters { get { return Model.Monsters; } set { Model.Monsters = value; } }
        private Monster monster { get { return monsters[Index]; } set { monsters[Index] = value; } }
        private FontCharacter[] fontMenu { get { return Model.FontMenu; } }
        private int[] fontPaletteBattle { get { return Model.FontPaletteBattle.Palettes[0]; } }
        public int Index { get { return (int)monsterNum.Value; } set { monsterNum.Value = value; } }
        private Settings settings = Settings.Default;
        private MenuTextPreview menuTextPreview = new MenuTextPreview();
        //
        private BattleScripts battleScriptsEditor;
        private HackingTools hackingToolsWindow;
        //
        private bool byteView { get { return !textView.Checked; } set { textView.Checked = !value; } }
        private Bitmap psychopathBGImage { get { return Model.BattleDialogueTilesetImage; } }
        private Bitmap psychopathTextImage;
        private TextHelper textHelper = TextHelper.Instance;
        private BattleDialoguePreview battleDialoguePreview = new BattleDialoguePreview();
        private FontCharacter[] fontDialogue { get { return Model.FontDialogue; } }
        private int[] fontPaletteDialogue { get { return Model.FontPaletteDialogue.Palettes[1]; } }
        #endregion
        #region Functions
        public Monsters()
        {
            settings.Keystrokes[0x20] = "\x20";
            settings.KeystrokesMenu[0x20] = "\x20";
            InitializeComponent();
            Do.AddShortcut(toolStrip4, Keys.Control | Keys.S, new EventHandler(save_Click));
            Do.AddShortcut(toolStrip4, Keys.F1, helpTips);
            Do.AddShortcut(toolStrip4, Keys.F2, baseConvertor);
            // create editors
            battleScriptsEditor = new BattleScripts(this);
            battleScriptsEditor.TopLevel = false;
            battleScriptsEditor.Dock = DockStyle.Fill;
            //battleScriptsEditor.SetToolTips(toolTip1);
            hackingToolsWindow = new HackingTools(new Function(RefreshMonsterTab), monsterNum);
            panel1.Controls.Add(battleScriptsEditor);
            battleScriptsEditor.BringToFront();
            battleScriptsEditor.Visible = true;

            toolTip1.InitialDelay = 0;
            InitializeStrings();
            RefreshMonsterTab();
            new ToolTipLabel(this, baseConvertor, helpTips);
            new History(this);
            //
            if (settings.RememberLastIndex)
                Index = settings.LastMonster;
            checksum = Do.GenerateChecksum(monsters);
            //MessageBox.Show(battleScriptsEditor.Width + " x " + battleScriptsEditor.Height);
        }
        private void InitializeStrings()
        {
            // monster names
            this.monsterName.Items.Clear();
            this.monsterName.Items.AddRange(Model.MonsterNames.Names);
            // item names
            this.MonsterYoshiCookie.Items.Clear();
            this.MonsterYoshiCookie.Items.AddRange(Model.ItemNames.Names);
            this.ItemWinA.Items.Clear();
            this.ItemWinA.Items.AddRange(Model.ItemNames.Names);
            this.ItemWinB.Items.Clear();
            this.ItemWinB.Items.AddRange(Model.ItemNames.Names);
        }
        private void RefreshMonsterTab()
        {
            if (!updatingMonsters)
            {
                Cursor.Current = Cursors.WaitCursor;
                updatingMonsters = true;
                this.monsterName.SelectedIndex = Model.MonsterNames.GetIndexFromNum(Index);
                this.monsterNameText.Text = Do.RawToASCII(monster.Name, settings.KeystrokesMenu);
                this.MonsterValHP.Value = monster.HP;
                this.MonsterValSpeed.Value = monster.Speed;
                this.MonsterValAtk.Value = monster.Attack;
                this.MonsterValMgDef.Value = monster.MagicDefense;
                this.MonsterValMgAtk.Value = monster.MagicAttack;
                this.MonsterValDef.Value = monster.Defense;
                this.MonsterValMgEvd.Value = monster.MagicEvadePercent;
                this.MonsterValEvd.Value = monster.EvadePercent;
                this.MonsterValFP.Value = monster.FP;
                this.MonsterValExp.Value = monster.Experience;
                this.MonsterValCoins.Value = monster.Coins;
                this.MonsterValElevation.Value = monster.Elevation;
                this.MonsterFlowerOdds.Value = monster.FlowerOdds * 10;
                this.MonsterElementsNullify.SetItemChecked(0, monster.ElemIceNull);
                this.MonsterElementsNullify.SetItemChecked(1, monster.ElemFireNull);
                this.MonsterElementsNullify.SetItemChecked(2, monster.ElemThunderNull);
                this.MonsterElementsNullify.SetItemChecked(3, monster.ElemJumpNull);
                this.MonsterProperties.SetItemChecked(0, monster.Invincible);
                this.MonsterProperties.SetItemChecked(1, monster.MortalityProtection);
                this.MonsterProperties.SetItemChecked(2, monster.DisableAutoDeath);
                this.MonsterProperties.SetItemChecked(3, monster.Palette2bpp);
                this.MonsterEffectsNullify.SetItemChecked(0, monster.EffectMuteNull);
                this.MonsterEffectsNullify.SetItemChecked(1, monster.EffectSleepNull);
                this.MonsterEffectsNullify.SetItemChecked(2, monster.EffectPoisonNull);
                this.MonsterEffectsNullify.SetItemChecked(3, monster.EffectFearNull);
                this.MonsterEffectsNullify.SetItemChecked(4, monster.EffectMushroomNull);
                this.MonsterEffectsNullify.SetItemChecked(5, monster.EffectScarecrowNull);
                this.MonsterEffectsNullify.SetItemChecked(6, monster.EffectInvincibleNull);
                this.MonsterElementsWeakness.SetItemChecked(0, monster.ElemIceWeak);
                this.MonsterElementsWeakness.SetItemChecked(1, monster.ElemFireWeak);
                this.MonsterElementsWeakness.SetItemChecked(2, monster.ElemThunderWeak);
                this.MonsterElementsWeakness.SetItemChecked(3, monster.ElemJumpWeak);
                this.MonsterFlowerBonus.SelectedIndex = monster.FlowerBonus;
                this.MonsterMorphSuccess.SelectedIndex = monster.MorphSuccessRate;
                this.MonsterCoinSize.SelectedIndex = monster.CoinSize;
                this.MonsterBehavior.SelectedIndex = monster.SpriteBehavior;
                this.MonsterEntranceStyle.SelectedIndex = monster.EntranceStyle;
                this.MonsterSoundOther.SelectedIndex = monster.OtherSound;
                this.MonsterSoundStrike.SelectedIndex = monster.StrikeSound;
                this.MonsterYoshiCookie.SelectedIndex = Model.ItemNames.GetIndexFromNum(monster.YoshiCookie);
                this.ItemWinA.SelectedIndex = Model.ItemNames.GetIndexFromNum(monster.ItemWinA);
                this.ItemWinB.SelectedIndex = Model.ItemNames.GetIndexFromNum(monster.ItemWinB);
                //
                this.MonsterPsychopath.Text = monster.GetPsychoMsg(byteView);
                this.MonsterPsychopath_TextChanged(null, null);
                CalculateFreeSpace();
                SetDialogueImages();
                //
                updatingMonsters = false;
                Cursor.Current = Cursors.Arrow;
            }
        }
        private void InsertIntoText(string toInsert)
        {
            char[] newText = new char[MonsterPsychopath.Text.Length + toInsert.Length];

            MonsterPsychopath.Text.CopyTo(0, newText, 0, MonsterPsychopath.SelectionStart);
            toInsert.CopyTo(0, newText, MonsterPsychopath.SelectionStart, toInsert.Length);
            MonsterPsychopath.Text.CopyTo(MonsterPsychopath.SelectionStart, newText, MonsterPsychopath.SelectionStart + toInsert.Length, this.MonsterPsychopath.Text.Length - this.MonsterPsychopath.SelectionStart);
            if (byteView)
                monster.CaretPositionByteView = this.MonsterPsychopath.SelectionStart + toInsert.Length;
            else
                monster.CaretPositionTextView = this.MonsterPsychopath.SelectionStart + toInsert.Length;
            monster.SetPsychoMsg(new string(newText), byteView);
            this.MonsterPsychopath.Text = monster.GetPsychoMsg(byteView);
            if (byteView)
                this.MonsterPsychopath.SelectionStart = monster.CaretPositionByteView;
            else
                this.MonsterPsychopath.SelectionStart = monster.CaretPositionTextView;
        }
        private void CalculateFreeSpace()
        {
            int used = 0; int size = 0xb641 + 0x2229;
            for (int i = 0; i < monsters.Length - 1; i++)
            {
                used += monsters[i].RawPsychoMsg.Length;
                if (used + monsters[i].RawPsychoMsg.Length > size)
                {
                    bool test = size >= used + monsters[i].RawPsychoMsg.Length;
                    if (!test)
                    {
                        freeBytes.Text = "Entry " + i++.ToString() + " Too Long - Cannot Save";
                        return;
                    }
                }
            }
            freeBytes.Text = ((double)(size - used)).ToString() + " characters left";
        }
        private void SetDialogueImages()
        {
            pictureBoxPsychopath.BackColor = Color.FromArgb(fontPaletteDialogue[0]);
            pictureBoxPsychopath.Invalidate();
            MonsterPsychopath_TextChanged(null, null);
        }
        public void Assemble()
        {
            int i = 0;
            ushort len = 0xa1d1;
            for (; i < monsters.Length && len + monsters[i].RawPsychoMsg.Length < 0xb641; i++)
                len += monsters[i].Assemble(len);
            len = 0x1c2a;
            for (; i < monsters.Length && len + monsters[i].RawPsychoMsg.Length < 0x2229; i++)
                len += monsters[i].Assemble(len);
            if (i != monsters.Length)
                System.Windows.Forms.MessageBox.Show(
                    "The allotted space for psychopath dialogues has been exceeded. Not all psychopath dialogues have been saved.",
                    "LAZY SHELL");
            battleScriptsEditor.Assemble();
            checksum = Do.GenerateChecksum(monsters);
        }
        #endregion
        #region Event Handlers
        private void Monsters_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Do.GenerateChecksum(monsters) == checksum && battleScriptsEditor.ChecksumNotChanged())
            {
                if (hackingToolsWindow.Visible) hackingToolsWindow.Close();
                return;
            }
            DialogResult result = MessageBox.Show(
                "Monsters and battlescripts have not been saved.\n\nWould you like to save changes?", "LAZY SHELL",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Assemble();
                battleScriptsEditor.Assemble();
                battleScriptsEditor.Close();
                if (hackingToolsWindow.Visible) hackingToolsWindow.Close();
            }
            else if (result == DialogResult.No)
            {
                Model.Monsters = null;
                Model.MonsterNames = null;
                Model.BattleScripts = null;
                battleScriptsEditor.Close();
                if (hackingToolsWindow.Visible) hackingToolsWindow.Close();
                return;
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
        }
        // main
        private void monsterNum_ValueChanged(object sender, EventArgs e)
        {
            RefreshMonsterTab();
            battleScriptsEditor.InitializeBattleScriptsEditor();
            settings.LastMonster = Index;
        }
        private void monsterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.monsterNum.Value = Model.MonsterNames.GetNumFromIndex(monsterName.SelectedIndex);
        }
        private void monsterName_DrawItem(object sender, DrawItemEventArgs e)
        {
            Do.DrawName(sender, e, menuTextPreview, Model.MonsterNames, fontMenu, fontPaletteBattle, true, Model.MenuBG_);
        }
        private void monsterNameText_TextChanged(object sender, EventArgs e)
        {
            if (Model.MonsterNames.GetNameByNum(monster.Index).CompareTo(this.monsterNameText.Text) != 0)
            {
                monster.Name = Do.ASCIIToRaw(this.monsterNameText.Text, settings.KeystrokesMenu, 13);

                Model.MonsterNames.SwapName(
                    monster.Index,
                    new string(monster.Name));
                Model.MonsterNames.SortAlpha();

                this.monsterName.Items.Clear();
                this.monsterName.Items.AddRange(Model.MonsterNames.GetNames());
                this.monsterName.SelectedIndex = Model.MonsterNames.GetIndexFromNum(monster.Index);
            }
        }
        // vital stats
        private void MonsterValHP_ValueChanged(object sender, EventArgs e)
        {
            monster.HP = (ushort)MonsterValHP.Value;
        }
        private void MonsterValFP_ValueChanged(object sender, EventArgs e)
        {
            monster.FP = (byte)MonsterValFP.Value;
        }
        private void MonsterValAtk_ValueChanged(object sender, EventArgs e)
        {
            monster.Attack = (byte)MonsterValAtk.Value;
        }
        private void MonsterValDef_ValueChanged(object sender, EventArgs e)
        {
            monster.Defense = (byte)MonsterValDef.Value;
        }
        private void MonsterValMgAtk_ValueChanged(object sender, EventArgs e)
        {
            monster.MagicAttack = (byte)MonsterValMgAtk.Value;
        }
        private void MonsterValMgDef_ValueChanged(object sender, EventArgs e)
        {
            monster.MagicDefense = (byte)MonsterValMgDef.Value;
        }
        private void MonsterValSpeed_ValueChanged(object sender, EventArgs e)
        {
            monster.Speed = (byte)MonsterValSpeed.Value;
        }
        private void MonsterValEvd_ValueChanged(object sender, EventArgs e)
        {
            monster.EvadePercent = (byte)MonsterValEvd.Value;
        }
        private void MonsterValMgEvd_ValueChanged(object sender, EventArgs e)
        {
            monster.MagicEvadePercent = (byte)MonsterValMgEvd.Value;
        }
        // reward stats
        private void MonsterValExp_ValueChanged(object sender, EventArgs e)
        {
            monster.Experience = (ushort)MonsterValExp.Value;
        }
        private void MonsterValCoins_ValueChanged(object sender, EventArgs e)
        {
            monster.Coins = (byte)MonsterValCoins.Value;
        }
        private void itemName_DrawItem(object sender, DrawItemEventArgs e)
        {
            Do.DrawName(
                sender, e, new BattleDialoguePreview(), Model.ItemNames, Model.FontMenu,
                Model.FontPaletteMenu.Palettes[0], 8, 10, 0, 128, true, true, Model.MenuBG_);
        }
        private void ItemWinA_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.ItemWinA = (byte)Model.ItemNames.GetNumFromIndex(ItemWinA.SelectedIndex);
        }
        private void ItemWinB_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.ItemWinB = (byte)Model.ItemNames.GetNumFromIndex(ItemWinB.SelectedIndex);
        }
        private void MonsterYoshiCookie_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.YoshiCookie = (byte)Model.ItemNames.GetNumFromIndex(MonsterYoshiCookie.SelectedIndex);
        }
        // other properties
        private void MonsterMorphSuccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.MorphSuccessRate = (byte)MonsterMorphSuccess.SelectedIndex;
        }
        private void MonsterCoinSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.CoinSize = (byte)MonsterCoinSize.SelectedIndex;
        }
        private void MonsterEntranceStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.EntranceStyle = (byte)MonsterEntranceStyle.SelectedIndex;
        }
        private void MonsterBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.SpriteBehavior = (byte)MonsterBehavior.SelectedIndex;
        }
        private void MonsterSoundStrike_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.StrikeSound = (byte)MonsterSoundStrike.SelectedIndex;
        }
        private void MonsterSoundOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.OtherSound = (byte)MonsterSoundOther.SelectedIndex;
        }
        private void MonsterValElevation_ValueChanged(object sender, EventArgs e)
        {
            monster.Elevation = (byte)MonsterValElevation.Value;
        }
        // effects, elements
        private void MonsterEffectsNullify_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.EffectMuteNull = MonsterEffectsNullify.GetItemChecked(0);
            monster.EffectSleepNull = MonsterEffectsNullify.GetItemChecked(1);
            monster.EffectPoisonNull = MonsterEffectsNullify.GetItemChecked(2);
            monster.EffectFearNull = MonsterEffectsNullify.GetItemChecked(3);
            monster.EffectMushroomNull = MonsterEffectsNullify.GetItemChecked(4);
            monster.EffectScarecrowNull = MonsterEffectsNullify.GetItemChecked(5);
            monster.EffectInvincibleNull = MonsterEffectsNullify.GetItemChecked(6);
        }
        private void MonsterElementsWeakness_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.ElemIceWeak = MonsterElementsWeakness.GetItemChecked(0);
            monster.ElemFireWeak = MonsterElementsWeakness.GetItemChecked(1);
            monster.ElemThunderWeak = MonsterElementsWeakness.GetItemChecked(2);
            monster.ElemJumpWeak = MonsterElementsWeakness.GetItemChecked(3);
        }
        private void MonsterElementsNullify_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.ElemIceNull = MonsterElementsNullify.GetItemChecked(0);
            monster.ElemFireNull = MonsterElementsNullify.GetItemChecked(1);
            monster.ElemThunderNull = MonsterElementsNullify.GetItemChecked(2);
            monster.ElemJumpNull = MonsterElementsNullify.GetItemChecked(3);
        }
        private void MonsterProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.Invincible = MonsterProperties.GetItemChecked(0);
            monster.MortalityProtection = MonsterProperties.GetItemChecked(1);
            monster.DisableAutoDeath = MonsterProperties.GetItemChecked(2);
            monster.Palette2bpp = MonsterProperties.GetItemChecked(3);
        }
        private void MonsterFlowerBonus_SelectedIndexChanged(object sender, EventArgs e)
        {
            monster.FlowerBonus = (byte)MonsterFlowerBonus.SelectedIndex;
        }
        private void MonsterFlowerOdds_ValueChanged(object sender, EventArgs e)
        {
            if (MonsterFlowerOdds.Value % 10 != 0)
                MonsterFlowerOdds.Value = (int)MonsterFlowerOdds.Value / 10 * 10;
            else
                monster.FlowerOdds = (byte)(MonsterFlowerOdds.Value / 10);
        }
        // menustrip
        private void save_Click(object sender, EventArgs e)
        {
            Assemble();
        }
        private void import_Click(object sender, EventArgs e)
        {
            new IOElements(monsters, Index, "IMPORT MONSTERS...").ShowDialog();
            RefreshMonsterTab();
        }
        private void export_Click(object sender, EventArgs e)
        {
            new IOElements(monsters, Index, "EXPORT MONSTERS...").ShowDialog();
        }
        private void clear_Click(object sender, EventArgs e)
        {
            new ClearElements(monsters, Index, "CLEAR MONSTERS...").ShowDialog();
            RefreshMonsterTab();
        }
        private void importBattleScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            battleScriptsEditor.Import();
        }
        private void exportBattleScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            battleScriptsEditor.Export();
        }
        private void clearBattleScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            battleScriptsEditor.Clear();
        }
        private void hackingTools_Click(object sender, EventArgs e)
        {
            if (!hackingToolsWindow.Visible)
                hackingToolsWindow.Show();
            hackingToolsWindow.BringToFront();
        }
        private void resetCurrentMonsterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You're about to undo all changes to the current monster. Go ahead with reset?",
                "LAZY SHELL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            monster = new Monster(Model.Data, Index);
            monsterNum_ValueChanged(null, null);
        }
        private void resetCurrentBattleScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You're about to undo all changes to the current battle script. Go ahead with reset?",
                "LAZY SHELL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            battleScriptsEditor.BattleScript = new LAZYSHELL.ScriptsEditor.BattleScript(Model.Data, battleScriptsEditor.index);
            monsterNum_ValueChanged(null, null);
        }
        private void showMonster_Click(object sender, EventArgs e)
        {
            panel13.Visible = !panel13.Visible;
        }
        private void showBattleScripts_Click(object sender, EventArgs e)
        {
            battleScriptsEditor.Visible = !battleScriptsEditor.Visible;
        }
        // psychopath dialogue
        private void MonsterPsychopath_TextChanged(object sender, EventArgs e)
        {
            char[] text = MonsterPsychopath.Text.ToCharArray();
            char[] swap;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    int tempSel = MonsterPsychopath.SelectionStart;
                    swap = new char[text.Length + 2];
                    for (int x = 0; x < i; x++)
                        swap[x] = text[x];
                    swap[i] = '[';
                    swap[i + 1] = '1';
                    swap[i + 2] = ']';
                    for (int x = i + 3; x < swap.Length; x++)
                        swap[x] = text[x - 2];
                    MonsterPsychopath.Text = new string(swap);
                    text = MonsterPsychopath.Text.ToCharArray();
                    i += 2;
                    MonsterPsychopath.SelectionStart = tempSel + 2;
                }
            }

            bool flag = textHelper.VerifyCorrectSymbols(this.MonsterPsychopath.Text.ToCharArray(), byteView);
            if (flag)
            {
                this.MonsterPsychopath.BackColor = SystemColors.Window;
                monster.SetPsychoMsg(this.MonsterPsychopath.Text, byteView);

                if (!monster.PsychoMsgError)
                {
                    monster.SetPsychoMsg(MonsterPsychopath.Text, byteView);
                    int[] pixels = battleDialoguePreview.GetPreview(fontDialogue, fontPaletteDialogue, monster.RawPsychoMsg, false);

                    psychopathTextImage = new Bitmap(Do.PixelsToImage(pixels, 256, 32));
                    pictureBoxPsychopath.Invalidate();
                }
            }
            if (!flag || monster.PsychoMsgError)
                this.MonsterPsychopath.BackColor = Color.Red;
            CalculateFreeSpace();
        }
        private void pictureBoxPsychopath_Paint(object sender, PaintEventArgs e)
        {
            if (psychopathBGImage != null)
                e.Graphics.DrawImage(psychopathBGImage, 0, 0);
            if (psychopathTextImage != null)
                e.Graphics.DrawImage(psychopathTextImage, 0, 0);
        }
        private void pageUp_Click(object sender, EventArgs e)
        {
            battleDialoguePreview.PageUp();
            MonsterPsychopath_TextChanged(null, null);
        }
        private void pageDown_Click(object sender, EventArgs e)
        {
            battleDialoguePreview.PageDown(monster.RawPsychoMsg.Length);
            MonsterPsychopath_TextChanged(null, null);
        }
        private void byteOrTextView_Click(object sender, EventArgs e)
        {
            MonsterPsychopath.Text = monster.GetPsychoMsg(byteView);
        }
        private void newLine_Click(object sender, EventArgs e)
        {
            if (byteView)
                InsertIntoText("[1]");
            else
                InsertIntoText("[newLine]");
        }
        private void endString_Click(object sender, EventArgs e)
        {
            if (byteView)
                InsertIntoText("[0]");
            else
                InsertIntoText("[end]");
        }
        private void pause60f_Click(object sender, EventArgs e)
        {
            if (byteView)
                InsertIntoText("[12]");
            else
                InsertIntoText("[delay]");
        }
        private void pauseA_Click(object sender, EventArgs e)
        {
            if (byteView)
                InsertIntoText("[2]");
            else
                InsertIntoText("[pauseInput]");
        }
        private void pauseFrames_Click(object sender, EventArgs e)
        {
            if (byteView)
                InsertIntoText("[3]");
            else
                InsertIntoText("[delayInput]");
        }
        //
        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, panel13.ClientRectangle, Border3DStyle.Raised, Border3DSide.All);
        }
        #endregion
    }
}
