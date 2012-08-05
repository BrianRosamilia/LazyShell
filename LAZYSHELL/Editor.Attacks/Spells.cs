﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LAZYSHELL.Properties;

namespace LAZYSHELL
{
    public partial class Spells : Form
    {
        #region Variables

        private bool updating = false;
        private Spell[] spells { get { return Model.Spells; } set { Model.Spells = value; } }
        private Spell spell { get { return spells[index]; } set { spells[index] = value; } }
        public Spell Spell { get { return spell; } set { spell = value; } }
        private int index { get { return (int)spellNum.Value; } set { spellNum.Value = value; } }
        public int Index { get { return index; } set { index = value; } }
        private Settings settings = Settings.Default;
        private TextHelperReduced textHelper = TextHelperReduced.Instance;
        private bool textCodeFormat { get { return !byteOrText.Checked; } set { byteOrText.Checked = !value; } }
        private Bitmap descriptionFrame;
        private Bitmap descriptionText;
        #endregion
        // constructor
        public Spells()
        {
            this.settings.KeystrokesMenu[0x20] = "\x20";
            this.settings.KeystrokesDesc[0x20] = "\x20";
            InitializeComponent();
            InitializeStrings();
            RefreshSpells();
            RefreshTimingSpellsTwo();
        }
        #region Functions
        public void RefreshSpells()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (updating) return;
            updating = true;
            this.spellName.SelectedIndex = Model.SpellNames.GetIndexFromNum(index);
            this.spellFPCost.Value = spell.FPCost;
            this.spellMagPower.Value = spell.MagicPower;
            this.spellHitRate.Value = spell.HitRate;
            this.spellNameIcon.SelectedIndex = (int)(spell.Name[0] - 0x20);
            this.spellNameIcon.Invalidate();
            if (index < 64)
                this.textBoxSpellName.Text = Do.RawToASCII(spell.Name, settings.KeystrokesMenu).Substring(1);
            else
                this.textBoxSpellName.Text = Do.RawToASCII(spell.Name, settings.Keystrokes).Substring(1);
            if (index > 26)
            {
                this.textBoxSpellDescription.Text = " This spell[1] cannot have a[1] description";
                if (spell.RawDescription == null)
                    this.textBoxSpellDescription_TextChanged(null, null);
                this.textBoxSpellDescription.Enabled = false;
                this.toolStrip2.Enabled = false;
            }
            else
            {
                if (this.textBoxSpellDescription.Enabled == false)
                {
                    this.textBoxSpellDescription.Enabled = true;
                    this.toolStrip2.Enabled = true;
                }
                if (index <= 26)
                {
                    this.textBoxSpellDescription.Enabled = true;
                    this.toolStrip2.Enabled = true;
                    this.textBoxSpellDescription.Text = spell.GetDescription(textCodeFormat);
                }
                else
                {
                    this.textBoxSpellDescription.Enabled = false;
                    this.toolStrip2.Enabled = false;
                    this.textBoxSpellDescription.Text = " This spell[1] cannot have a[1] description";
                }
            }
            this.spellAttackProp.SetItemChecked(0, spell.CheckStats);
            this.spellAttackProp.SetItemChecked(1, spell.IgnoreDefense);
            this.spellAttackProp.SetItemChecked(2, spell.CheckMortality);
            this.spellAttackProp.SetItemChecked(3, spell.UsableOverworld);
            this.spellAttackProp.SetItemChecked(4, spell.MaxAttack);
            this.spellAttackProp.SetItemChecked(5, spell.HideDigits);
            this.spellStatusEffect.SetItemChecked(0, spell.EffectMute);
            this.spellStatusEffect.SetItemChecked(1, spell.EffectSleep);
            this.spellStatusEffect.SetItemChecked(2, spell.EffectPoison);
            this.spellStatusEffect.SetItemChecked(3, spell.EffectFear);
            this.spellStatusEffect.SetItemChecked(4, spell.EffectMushroom);
            this.spellStatusEffect.SetItemChecked(5, spell.EffectScarecrow);
            this.spellStatusEffect.SetItemChecked(6, spell.EffectInvincible);
            this.spellStatusChange.SetItemChecked(0, spell.ChangeAttack);
            this.spellStatusChange.SetItemChecked(1, spell.ChangeDefense);
            this.spellStatusChange.SetItemChecked(2, spell.ChangeMagicAttack);
            this.spellStatusChange.SetItemChecked(3, spell.ChangeMagicDefense);
            this.spellTargetting.SetItemChecked(0, spell.TargetLiveAlly);
            this.spellTargetting.SetItemChecked(1, spell.TargetEnemy);
            this.spellTargetting.SetItemChecked(2, spell.TargetAll);
            this.spellTargetting.SetItemChecked(3, spell.TargetWoundedOnly);
            this.spellTargetting.SetItemChecked(4, spell.TargetOnePartyOnly);
            this.spellTargetting.SetItemChecked(5, spell.TargetNotSelf);
            this.spellAttackType.SelectedIndex = spell.AttackType;
            this.spellEffectType.SelectedIndex = spell.EffectType;
            this.spellFunction.SelectedIndex = spell.InflictFunction;
            this.spellInflictElement.SelectedIndex = spell.InflictElement;
            if (spell.EffectType == 0)
            {
                this.groupBox8.Text = "Effect <INFLICT>";
                this.groupBox9.Text = "Status <UP>";
            }
            else if (spell.EffectType == 1)
            {
                this.groupBox8.Text = "Effect <NULLIFY>";
                this.groupBox9.Text = "Status <DOWN>";
            }
            else if (spell.EffectType == 2)
            {
                this.groupBox8.Text = "Effect <. . . .>";
                this.groupBox9.Text = "Status <. . . .>";
            }
            // timing
            panelTimingMultiple.Visible = index == 2 || index == 4 || index == 26;
            panelTimingOne.Visible = index == 9 || index == 17 || index == 18 || index == 21 || index == 23;
            panelTimingTwo.Visible = index == 0 || index == 6 || index == 7 || index == 14 || index == 22 || index == 24;
            panelTimingGeno.Visible = index == 16 || index == 19 || index == 20;
            panelTimingRotation.Visible = index == 8 || index == 10 || index == 12 || index == 13 || index == 25;
            panelTimingRapid.Visible = index == 11 || index == 15;
            panelTimingFireball.Visible = index == 1 || index == 3 || index == 5;
            if (index == 2 || index == 4 || index == 26)
                RefreshTimingMultipleTiming();
            if (index == 9 || index == 17 || index == 18 || index == 21 || index == 23)
                RefreshTimingSpellsOne();
            if (index == 0 || index == 6 || index == 7 || index == 14 || index == 22 || index == 24)
                RefreshTimingSpellsTwo();
            if (index == 16 || index == 19 || index == 20)
                RefreshTimingSpellsGeno();
            if (index == 8 || index == 10 || index == 12 || index == 13 || index == 25)
                RefreshTimingRotaionSpells();
            if (index == 11 || index == 15)
                RefreshTimingRapidSpellMax();
            if (index == 1 || index == 3 || index == 5)
                RefreshTimingFireballSpells();
            updating = false;
            Cursor.Current = Cursors.Arrow;
        }
        private void RefreshTimingSpellsOne()
        {
            this.timingOne.Value = spell.OneLevelSpellSpan;
        }
        private void RefreshTimingSpellsTwo()
        {
            this.timingTwoStart.Value = spell.TwoLevelSpellStartLevel2;
            this.timingTwoEnd.Value = spell.TwoLevelSpellEndLevel2;
            this.timingOneEnd.Value = spell.TwoLevelSpellEndLevel1;
        }
        private void RefreshTimingSpellsGeno()
        {
            this.timingGeno2Frame.Value = spell.ChargeSpellStartLevel2;
            this.timingGeno3Frame.Value = spell.ChargeSpellStartLevel3;
            this.timingGeno4Frame.Value = spell.ChargeSpellStartLevel4;
            this.timingGenoOverflow.Value = spell.ChargeSpellOverflow;
        }
        private void RefreshTimingFireballSpells()
        {
            this.timingFireballSpan.Value = spell.FireballSpellRange;
            this.timingFireballMax.Value = spell.FireballSpellOrbs;
        }
        private void RefreshTimingRotaionSpells()
        {
            this.timingRotationStart.Value = spell.RotationSpellStart;
            this.timingRotationMax.Value = spell.RotationSpellMax;
        }
        private void RefreshTimingRapidSpellMax()
        {
            this.timingRapid.Value = spell.RapidSpellMax;
        }
        private void RefreshTimingMultipleTiming()
        {
            this.timingInstanceNumber.Items.Clear();
            if (index == 2 || index == 4)
            {
                int count = (index == 2) ? 14 : 17;
                for (int i = 0; i < count; i++)
                    this.timingInstanceNumber.Items.Add(
                        this.spellName.SelectedItem.ToString().Trim() + " " + i.ToString());
                this.timingInstanceNumber.SelectedIndex = 0;
                this.timingInstanceNumber.Enabled = true;
            }
            else
            {
                this.timingInstanceNumber.Items.Add("");
                this.timingInstanceNumber.SelectedIndex = 0;
                this.timingInstanceNumber.Enabled = false;
            }
            this.timingInstanceMax.Value = spell.MultipleSpellInstanceMax;
            this.timingInstanceSpan.Value = spell.MultipleSpellInstanceRange[timingInstanceNumber.SelectedIndex];
        }
        private void InitializeStrings()
        {
            this.spellName.Items.Clear();
            this.spellName.Items.AddRange(Model.SpellNames.Names);
            this.spellName.SelectedIndex = Model.SpellNames.GetIndexFromNum(index);
            string[] temp = new string[96];
            for (int i = 0; i < 96; i++)
                temp[i] = i.ToString();
            this.spellNameIcon.Items.Clear();
            this.spellNameIcon.Items.AddRange(temp);
        }
        private void InsertIntoDescriptionText(string toInsert)
        {
            char[] newText = new char[textBoxSpellDescription.Text.Length + toInsert.Length];
            textBoxSpellDescription.Text.CopyTo(0, newText, 0, textBoxSpellDescription.SelectionStart);
            toInsert.CopyTo(0, newText, textBoxSpellDescription.SelectionStart, toInsert.Length);
            textBoxSpellDescription.Text.CopyTo(textBoxSpellDescription.SelectionStart, newText, textBoxSpellDescription.SelectionStart + toInsert.Length, this.textBoxSpellDescription.Text.Length - this.textBoxSpellDescription.SelectionStart);
            if (textCodeFormat)
                spell.CaretPositionSymbol = this.textBoxSpellDescription.SelectionStart + toInsert.Length;
            else
                spell.CaretPositionNotSymbol = this.textBoxSpellDescription.SelectionStart + toInsert.Length;
            spell.SetDescription(new string(newText), textCodeFormat);
            textBoxSpellDescription.Text = spell.GetDescription(textCodeFormat);
        }
        public void SetToolTips(ToolTip toolTip1)
        {
            // SPELLS
            this.spellNum.ToolTipText =
                "Select the spell to edit by #. These properties are applied\n" +
                "to either in-battle usage, overworld usage or both.\n\n" +
                "Spell #0-31 are ally spells, while all other spells are monster\n" +
                "spells. Both are exclusively limited to usage by either allies\n" +
                "or monsters. Any attempts to assign monster spells to allies\n" +
                "or vice versa will most likely cause a glitch, and is not\n" +
                "recommended.";
            this.spellName.ToolTipText =
                "Select the spell to edit by name. These properties are\n" +
                "to either in-battle usage, overworld usage or both.\n\n" +
                "Spell #0-31 are ally spells, while all other spells are monster\n" +
                "spells. Both are exclusively limited to usage by either allies\n" +
                "or monsters. Any attempts to assign monster spells to allies\n" +
                "or vice versa will most likely cause a glitch, and is not\n" +
                "recommended.";
            this.textBoxSpellName.ToolTipText =
                "The spell's displayed name in all menus.";
            toolTip1.SetToolTip(this.spellFPCost,
                "The amount of FP subtracted from the user's current FP\n" +
                "when the spell is used.");
            toolTip1.SetToolTip(this.spellMagPower,
                "The base damage or heal amount caused by the spell.");
            toolTip1.SetToolTip(this.spellHitRate,
                "The spell's hit rate percent, ie. the probability out of 100\n" +
                "the spell will hit its target.");
            toolTip1.SetToolTip(this.spellAttackType,
                "The spell's attack type, ie. the spell will either cause\n" +
                "damage or heal its target. This property can be ignored\n" +
                "depending on the value of \"Inflict Function\".");
            toolTip1.SetToolTip(this.spellEffectType,
                "The effect type, ie. whether or not the spell will inflict or\n" +
                "nullify (an) effect(s). Example: Poison Gas inflicts the\n" +
                "Poison effect on the target(s) and Group Hug nullifies all\n" +
                "adverse effects on the target(s). If set to {NONE} then\n" +
                "anything checked under \"EFFECT\" is ignored. Likewise, this\n" +
                "property is ignored if nothing under \"EFFECT\" is checked.");
            toolTip1.SetToolTip(this.spellFunction,
                "The inflict functions are specialized to certain spells, eg.\n" +
                "\"Scan/Show HP\" is specialized to Psychopath and \"Jump\n" +
                "Power\" is specialized to Jump. Some of these will cause the\n" +
                "\"Attack Type\" to be ignored, ie. the spell will neither cause\n" +
                "damage nor heal (eg. Psychopath).");
            toolTip1.SetToolTip(this.spellInflictElement,
                "The element assigned to the spell. If the target has a\n" +
                "strength against the element, the base damage of the spell\n" +
                "will be halved. If the target has a weakness against the\n" +
                "element, the base damage will be doubled. If the target\n" +
                "has a nullification property against the element, it will yield\n" +
                "0 damage.");
            toolTip1.SetToolTip(this.spellAttackProp,
                "\"Check Caster/Target Atk/Def\" will add to or subtract from\n" +
                "the base damage or heal amount of the spell based on the\n" +
                "target's attack and defense power instead of its magic\n" +
                "attack and magic defense power. By default, no spells have\n" +
                "this property enabled.\n\n" +
                "\"Ignore Target\'s Defense\" will not subtract the target's\n" +
                "magic defense power from the spell's base damage or heal\n" +
                "amount (ie. the spell's magic power).\n\n" +
                "\"Check Mortality Protection\" is redundant because the\n" +
                "game engine always checks anyways. Only the dummied\n" +
                "Knock Out spell has this enabled by default.\n\n" +
                "\"Usable in overworld menu\" allows the spell to be used out\n" +
                "of battle, ie. the overworld menu. This is normally reserved\n" +
                "for healing spells.\n\n" +
                "\"9999 Damage/Heal\" will kill the target in one strike, if the\n" +
                "spell does not miss. Only the dummied Knock Out spell has\n" +
                "this enabled by default.\n\n" +
                "\"Hide Battle Numerals\" will hide the damage or heal amount\n" +
                "total (ie. the numbers shown after an attack). This is\n" +
                "generally used by spells that cause 0 damage and are only\n" +
                "effect-based spells such as Sleepy Time, to avoid a\n" +
                "redundant \"0\" appearing.");
            toolTip1.SetToolTip(this.textBoxSpellDescription,
                "The description that appears in the lower-right corner of\n" +
                "the overworld menu when the cursor is on the spell.");
            toolTip1.SetToolTip(this.spellTargetting,
                "\"Other Targets\" will limit the target to a single ally or\n" +
                "enemy. This must NOT be checked with \"Entire Party\".\n\n" +
                "\"Enemy Party\" will allow the spell to target the opposing\n" +
                "party.\n\n" +
                "\"Entire Party\" will cause the spell to target all members of\n" +
                "either the ally party or enemy party. This must NOT be\n" +
                "checked with \"Other Targets\".\n\n" +
                "\"Wounded Only\" will limit the target to wounded members,\n" +
                "ie. members with currently 0 HP.\n\n" +
                "\"One Party Only\" will limit the target to only one party. By\n" +
                "default, all usable spells have this property enabled.\n" +
                "Uncheck at your own risk!\n\n" +
                "\"Not Self\" will limit the target to other allies only, and the\n" +
                "caster is untargettable. By default no spells have this\n" +
                "checked, although the Mushroom item that turns the user\n" +
                "into a mushroom has this property enabled.");
            toolTip1.SetToolTip(this.spellStatusEffect,
                "The effect inflicted or nullified on a target, eg. Poison Gas\n" +
                "inflicts Poison on a target, while Group Hug will nullify all\n" +
                "effects a target is afflicted with except \"Invincible\". These\n" +
                "properties are used based on the value for \"Effect Type\".");
            toolTip1.SetToolTip(this.spellStatusChange,
                "The status of a target is either lowered or raised by 50%,\n" +
                "depending on the value for \"Effect Type\". If the value for\n" +
                "\"Effect Type\" is set to \"Inflict\" then the target's stats will be\n" +
                "raised 50%. If \"Effect type is set to \"Nullify\" then the\n" +
                "target's stats will be lowered 50%.\n\n" +
                "Example: Geno Boost by default raises the target's Attack\n" +
                "and Defense power by 50% (eg. if the attack and/or\n" +
                "defense power of the target is 100, then it becomes 150).\n" +
                "Shredder by default lowers the target's Attack, Defense,\n" +
                "Magic Attack, and Magic Defense power by 50% (ie. it\n" +
                "halves them).");
            // timing
            toolTip1.SetToolTip(this.timingOne,
                "The # of frames from the start of the spell's animation \n" +
                "when the user can trigger level 1 timing. The spell's damage \n" +
                "will be increased by 50% if an ABXY button is pressed \n" +
                "within this range. ");

            toolTip1.SetToolTip(this.timingTwoStart,
                "The frame # from the start of the spell animation where the \n" +
                "level 2 timing begins.\n" +
                "\n" +
                "Example: the default value for Jump is 39. This means that \n" +
                "if an ABXY button is pressed after 39 frames have passed \n" +
                "from the start of the Jump animation (ie. when Mario jumps \n" +
                "off the ground) the damage is increased by at least 100% \n" +
                "(ie. doubled).");
            toolTip1.SetToolTip(this.timingTwoEnd,
                "The frame # from the start of the spell animation when the \n" +
                "level 2 timing ends.\n" +
                "\n" +
                "Example: the default value for Jump is 44. This means that \n" +
                "if an ABXY button has NOT been pressed after 44 frames \n" +
                "have passed from the start of the Jump animation (ie. \n" +
                "when Mario jumps off the ground) the opportunity to \n" +
                "increase the damage by 100% (ie. doubled) is gone.");
            toolTip1.SetToolTip(this.timingOneEnd,
                "The frame # from the start of the spell animation where the \n" +
                "level 1 timing ends.\n" +
                "\n" +
                "Example: the default value for Jump is 45. This means that \n" +
                "if an ABXY button has NOT been pressed after 45 frames \n" +
                "have passed from the start of the Jump animation, the \n" +
                "opportunity to time the attack for any damage increase is \n" +
                "gone.");

            toolTip1.SetToolTip(this.timingGeno2Frame,
                "The frame # from the start of the spell animation when, if \n" +
                "the button is held to this point, the damage is increased by \n" +
                "at least 50%. This is by default around when the first red \n" +
                "star appears on screen.");
            toolTip1.SetToolTip(this.timingGeno3Frame,
                "The frame # from the start of the spell animation when, if \n" +
                "the button is held to this point, the damage is increased by \n" +
                "at least 75%. This is by default around when the second \n" +
                "red star appears on screen.");
            toolTip1.SetToolTip(this.timingGeno4Frame,
                "The frame # from the start of the spell animation when, if \n" +
                "the button is held to this point, the damage is increased by \n" +
                "at least 100%. This is by default around when the third red \n" +
                "star appears on screen.");
            toolTip1.SetToolTip(this.timingGenoOverflow,
                "The frame # from the start of the spell animation when, if \n" +
                "the button is held to this point and beyond, the damage \n" +
                "\"overflows\" and is reset to the base value, ie. no damage \n" +
                "increase.");

            toolTip1.SetToolTip(this.timingFireballSpan,
                "The \"speed\" of the firing, or the # of frames the player \n" +
                "must wait between button presses in order to \"fire\" another \n" +
                "fireball.\n" +
                "NOTE: values less than the default may cause the game to \n" +
                "freeze if the button is consistently pressed for each frame \n" +
                "span between fireballs.");
            toolTip1.SetToolTip(this.timingFireballMax,
                "The maximum number of orbs the player can fire before the \n" +
                "spell is over. The accumulative damage is increased with \n" +
                "each fireball, so lowering/raising this value will affect the \n" +
                "maximum accumulative damage as well.");

            toolTip1.SetToolTip(this.timingRotationStart,
                "The frame # from the start of the spell animation when the \n" +
                "player has the opportunity to rotate the directional pad to \n" +
                "increase damage.");
            toolTip1.SetToolTip(this.timingRotationMax,
                "The maximum number of quarter rotations (a quarter \n" +
                "rotation would be, for example, from DOWN to DOWN-LEFT \n" +
                "to LEFT) allowed to increase damage. Raising/lowering this \n" +
                "value will affect the maximum accumulative damage.");

            toolTip1.SetToolTip(this.timingInstanceMax,
                "The maximum number of times the player can execute \n" +
                "another \"jump\" or \"star rain\" by timing it. Values above 127 \n" +
                "will likely cause anomalies (ie. the spell caster might only be \n" +
                "able to do 13 jumps, even if the maximum is set to 200 for \n" +
                "example).");
            toolTip1.SetToolTip(this.timingInstanceNumber,
                "The instance selected. The rest of the instances have the \n" +
                "same \"Instance Frame Duration\" as the last one in the list \n" +
                "of instances. For example, Super Jump instances 14 \n" +
                "through 199 will have the same \"Instance Frame Duration\" \n" +
                "as instance 13.\n" +
                "NOTE: star rain's \"Instance Frame Duration\" is the same for \n" +
                "all instances, so there isn't a list for them.");
            toolTip1.SetToolTip(this.timingInstanceSpan,
                "The # of frames before either Mario or the Star lands on \n" +
                "the target that the player is able to time the spell to \n" +
                "increment damage and allow another instance to be timed.\n" +
                "NOTE: star rain's \"Instance Frame Duration\" is the same for \n" +
                "all instances, so there isn't a list for them.");

            toolTip1.SetToolTip(this.timingRapid,
                "The maximum number of times the player can press an \n" +
                "ABXY button to increase damage during the spell animation.");
        }
        #endregion
        #region Event Handlers
        private void spellNum_ValueChanged(object sender, EventArgs e)
        {
            RefreshSpells();
            settings.LastSpell = index;
        }
        private void spellName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.spellNum.Value = Model.SpellNames.GetNumFromIndex(spellName.SelectedIndex);
        }
        private void spellName_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            Do.DrawName(
                sender, e, new BattleDialoguePreview(), Model.SpellNames,
                Model.SpellNames.GetNumFromIndex(e.Index) < 64 ? Model.FontMenu : Model.FontDialogue,
                Model.FontPaletteMenu.Palettes[0], 8, 10, 0, 128, false, false, Model.MenuBG_);
        }
        private void spellNameIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.Name[0] = (char)(spellNameIcon.SelectedIndex + 0x20);
            if (Model.SpellNames.GetNameByNum(index).CompareTo(this.textBoxSpellName.Text) != 0)
            {
                Model.SpellNames.SwapName(
                    index, new string(spell.Name));
                Model.SpellNames.SortAlpha();
                this.spellName.Items.Clear();
                this.spellName.Items.AddRange(Model.SpellNames.GetNames());
                this.spellName.SelectedIndex = Model.SpellNames.GetIndexFromNum(index);
            }
        }
        private void spellNameIcon_DrawItem(object sender, DrawItemEventArgs e)
        {
            Do.DrawIcon(sender, e, new MenuTextPreview(), 32, Model.FontMenu, Model.FontPaletteMenu.Palettes[0], false, Model.MenuBG_);
        }
        private void textBoxSpellName_TextChanged(object sender, EventArgs e)
        {
            char[] raw = new char[15];
            char[] temp;
            if (spellNum.Value < 64)
                temp = Do.ASCIIToRaw(this.textBoxSpellName.Text, settings.KeystrokesMenu, 14);
            else
                temp = Do.ASCIIToRaw(this.textBoxSpellName.Text, settings.Keystrokes, 14);
            for (int i = 0; i < 14; i++)
                raw[i + 1] = temp[i];
            raw[0] = (char)(spellNameIcon.SelectedIndex + 32);
            if (Model.SpellNames.GetNameByNum(index).CompareTo(this.textBoxSpellName.Text) != 0)
            {
                spell.Name = raw;
                Model.SpellNames.SwapName(
                    index, new string(spell.Name));
                Model.SpellNames.SortAlpha();
                this.spellName.Items.Clear();
                this.spellName.Items.AddRange(Model.SpellNames.GetNames());
                this.spellName.SelectedIndex = Model.SpellNames.GetIndexFromNum(index);
            }
        }
        private void textBoxSpellName_Leave(object sender, EventArgs e)
        {
            spellName.Items.Clear();
            spellName.Items.AddRange(Model.SpellNames.GetNames());
            spellName.SelectedIndex = Model.SpellNames.GetIndexFromNum(index);
            InitializeStrings();
        }
        private void spellFPCost_ValueChanged(object sender, EventArgs e)
        {
            spell.FPCost = (byte)this.spellFPCost.Value;
        }
        private void spellMagPower_ValueChanged(object sender, EventArgs e)
        {
            spell.MagicPower = (byte)this.spellMagPower.Value;
        }
        private void spellHitRate_ValueChanged(object sender, EventArgs e)
        {
            spell.HitRate = (byte)this.spellHitRate.Value;
        }
        private void spellAttackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.AttackType = (byte)this.spellAttackType.SelectedIndex;
        }
        private void spellEffectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.EffectType = (byte)this.spellEffectType.SelectedIndex;

            if (spell.EffectType == 0)
            {
                this.groupBox8.Text = "Effect <INFLICT>";
                this.groupBox9.Text = "Status <UP>";
            }
            else if (spell.EffectType == 1)
            {
                this.groupBox8.Text = "Effect <NULLIFY>";
                this.groupBox9.Text = "Status <DOWN>";
            }
            else if (spell.EffectType == 2)
            {
                this.groupBox8.Text = "Effect <. . . .>";
                this.groupBox9.Text = "Status <. . . .>";
            }
        }
        private void spellFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.InflictFunction = (byte)this.spellFunction.SelectedIndex;
        }
        private void spellInflictElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.InflictElement = (byte)this.spellInflictElement.SelectedIndex;
        }
        private void spellAttackProp_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.CheckStats = this.spellAttackProp.GetItemChecked(0);
            spell.IgnoreDefense = this.spellAttackProp.GetItemChecked(1);
            spell.CheckMortality = this.spellAttackProp.GetItemChecked(2);
            spell.UsableOverworld = this.spellAttackProp.GetItemChecked(3);
            spell.MaxAttack = this.spellAttackProp.GetItemChecked(4);
            spell.HideDigits = this.spellAttackProp.GetItemChecked(5);
        }
        private void spellStatusEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.EffectMute = this.spellStatusEffect.GetItemChecked(0);
            spell.EffectSleep = this.spellStatusEffect.GetItemChecked(1);
            spell.EffectPoison = this.spellStatusEffect.GetItemChecked(2);
            spell.EffectFear = this.spellStatusEffect.GetItemChecked(3);
            spell.EffectMushroom = this.spellStatusEffect.GetItemChecked(4);
            spell.EffectScarecrow = this.spellStatusEffect.GetItemChecked(5);
            spell.EffectInvincible = this.spellStatusEffect.GetItemChecked(6);
        }
        private void spellTargetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.TargetLiveAlly = this.spellTargetting.GetItemChecked(0);
            spell.TargetEnemy = this.spellTargetting.GetItemChecked(1);
            spell.TargetAll = this.spellTargetting.GetItemChecked(2);
            spell.TargetWoundedOnly = this.spellTargetting.GetItemChecked(3);
            spell.TargetOnePartyOnly = this.spellTargetting.GetItemChecked(4);
            spell.TargetNotSelf = this.spellTargetting.GetItemChecked(5);
        }
        private void spellStatusChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            spell.ChangeAttack = this.spellStatusChange.GetItemChecked(0);
            spell.ChangeDefense = this.spellStatusChange.GetItemChecked(1);
            spell.ChangeMagicAttack = this.spellStatusChange.GetItemChecked(2);
            spell.ChangeMagicDefense = this.spellStatusChange.GetItemChecked(3);
        }
        private void textBoxSpellDescription_TextChanged(object sender, EventArgs e)
        {
            char[] text = textBoxSpellDescription.Text.ToCharArray();
            char[] swap;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    int tempSel = textBoxSpellDescription.SelectionStart;
                    swap = new char[text.Length + 2];
                    for (int x = 0; x < i; x++)
                        swap[x] = text[x];
                    swap[i] = '[';
                    swap[i + 1] = '1';
                    swap[i + 2] = ']';
                    for (int x = i + 3; x < swap.Length; x++)
                        swap[x] = text[x - 2];
                    textBoxSpellDescription.Text = new string(swap);
                    text = textBoxSpellDescription.Text.ToCharArray();
                    i += 2;
                    textBoxSpellDescription.SelectionStart = tempSel + 2;
                }
            }
            bool flag = textHelper.VerifyCorrectSymbols(this.textBoxSpellDescription.Text.ToCharArray(), textCodeFormat);
            if (flag)
            {
                this.textBoxSpellDescription.BackColor = Color.FromArgb(255, 255, 255, 255);
                spell.SetDescription(this.textBoxSpellDescription.Text, textCodeFormat);
            }
            if (!flag || spell.DescriptionError)
                this.textBoxSpellDescription.BackColor = Color.Red;
            descriptionText = null;
            pictureBoxSpellDesc.Invalidate();
        }
        private void newLine_Click(object sender, EventArgs e)
        {
            if (textCodeFormat)
                InsertIntoDescriptionText("[1]");
            else
                InsertIntoDescriptionText("[newLine]");
        }
        private void endString_Click(object sender, EventArgs e)
        {
            if (textCodeFormat)
                InsertIntoDescriptionText("[0]");
            else
                InsertIntoDescriptionText("[endInput]");
        }
        private void pictureBoxSpellDesc_Paint(object sender, PaintEventArgs e)
        {
            if (spell.RawDescription == null)
                return;
            if (descriptionFrame == null)
            {
                int[] bgPixels = Do.ImageToPixels(Model.MenuBG);
                Do.DrawMenuFrame(bgPixels, 256, new Rectangle(0, 0, 15, 8), Model.MenuFrameGraphics, Model.FontPaletteMenu.Palette);
                descriptionFrame = Do.PixelsToImage(bgPixels, 256, 256);
            }
            e.Graphics.DrawImage(descriptionFrame, 0, 0);
            if (descriptionText == null)
                SetDescriptionText();
            e.Graphics.DrawImage(descriptionText, 0, 0);
        }
        private void SetDescriptionText()
        {
            int[] pixels = new MenuDescriptionPreview().GetPreview(
                Model.FontDescription, Model.FontPaletteMenu.Palettes[0], spell.RawDescription,
                new Size(120, 88), new Point(8, 8), 6);
            descriptionText = new Bitmap(Do.PixelsToImage(pixels, 120, 88));
            pictureBoxSpellDesc.Invalidate();
        }
        private void byteOrText_Click(object sender, EventArgs e)
        {
            this.textBoxSpellDescription.Text = spell.GetDescription(textCodeFormat);
        }
        // level 1 timing
        private void numericUpDown100_ValueChanged(object sender, EventArgs e)
        {
            this.spell.OneLevelSpellSpan = (byte)timingOne.Value;
        }
        // level 2 timing
        private void numericUpDown107_ValueChanged(object sender, EventArgs e)
        {
            this.spell.TwoLevelSpellStartLevel2 = (byte)timingTwoStart.Value;
            if (index == 6)
                this.spells[7].TwoLevelSpellStartLevel2 = (byte)timingTwoStart.Value;
            if (index == 7)
                this.spells[6].TwoLevelSpellStartLevel2 = (byte)timingTwoStart.Value;
        }
        private void numericUpDown110_ValueChanged(object sender, EventArgs e)
        {
            this.spell.TwoLevelSpellEndLevel2 = (byte)timingTwoEnd.Value;
            if (index == 6)
                this.spells[7].TwoLevelSpellEndLevel2 = (byte)timingTwoEnd.Value;
            if (index == 7)
                this.spells[6].TwoLevelSpellEndLevel2 = (byte)timingTwoEnd.Value;
        }
        private void numericUpDown108_ValueChanged(object sender, EventArgs e)
        {
            this.spell.TwoLevelSpellEndLevel1 = (byte)timingOneEnd.Value;
            if (index == 6)
                this.spells[7].TwoLevelSpellEndLevel1 = (byte)timingOneEnd.Value;
            if (index == 7)
                this.spells[6].TwoLevelSpellEndLevel1 = (byte)timingOneEnd.Value;
        }
        // charge spell timing
        private void timingGeno2Frame_ValueChanged(object sender, EventArgs e)
        {
            this.spells[16].ChargeSpellStartLevel2 = (byte)timingGeno2Frame.Value;
            this.spells[19].ChargeSpellStartLevel2 = (byte)timingGeno2Frame.Value;
            this.spells[20].ChargeSpellStartLevel2 = (byte)timingGeno2Frame.Value;
        }
        private void timingGeno3Frame_ValueChanged(object sender, EventArgs e)
        {
            this.spells[16].ChargeSpellStartLevel3 = (byte)timingGeno3Frame.Value;
            this.spells[19].ChargeSpellStartLevel3 = (byte)timingGeno3Frame.Value;
            this.spells[20].ChargeSpellStartLevel3 = (byte)timingGeno3Frame.Value;
        }
        private void numericUpDown114_ValueChanged(object sender, EventArgs e)
        {
            this.spells[16].ChargeSpellStartLevel4 = (byte)timingGeno4Frame.Value;
            this.spells[19].ChargeSpellStartLevel4 = (byte)timingGeno4Frame.Value;
            this.spells[20].ChargeSpellStartLevel4 = (byte)timingGeno4Frame.Value;
        }
        private void numericUpDown112_ValueChanged(object sender, EventArgs e)
        {
            this.spells[16].ChargeSpellOverflow = (byte)timingGenoOverflow.Value;
            this.spells[19].ChargeSpellOverflow = (byte)timingGenoOverflow.Value;
            this.spells[20].ChargeSpellOverflow = (byte)timingGenoOverflow.Value;
        }
        // fireball timing
        private void numericUpDown106_ValueChanged(object sender, EventArgs e)
        {
            this.spell.FireballSpellRange = (byte)timingFireballSpan.Value;
        }
        private void numericUpDown105_ValueChanged(object sender, EventArgs e)
        {
            this.spell.FireballSpellOrbs = (byte)timingFireballMax.Value;
        }
        // rotation timing
        private void numericUpDown104_ValueChanged(object sender, EventArgs e)
        {
            this.spell.RotationSpellStart = (byte)timingRotationStart.Value;
        }
        private void numericUpDown103_ValueChanged(object sender, EventArgs e)
        {
            this.spell.RotationSpellMax = (byte)timingRotationMax.Value;
        }
        // multiple instance timing
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            this.spell.MultipleSpellInstanceMax = (byte)timingInstanceMax.Value;
        }
        private void instanceNumberName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.timingInstanceSpan.Value = spell.MultipleSpellInstanceRange[timingInstanceNumber.SelectedIndex];
        }
        private void instanceNumberName_DrawItem(object sender, DrawItemEventArgs e)
        {
            string[] array = Lists.Convert(timingInstanceNumber.Items);
            Do.DrawName(
                sender, e, new BattleDialoguePreview(), array, Model.FontMenu,
                Model.FontPaletteMenu.Palettes[0], 8, 10, 0, 128, false, false, Model.MenuBG_);
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            this.spell.MultipleSpellInstanceRange[timingInstanceNumber.SelectedIndex] = (byte)timingInstanceSpan.Value;
        }
        // rapid spell timing
        private void numericUpDown102_ValueChanged(object sender, EventArgs e)
        {
            this.spells[11].RapidSpellMax = (byte)timingRapid.Value;
            this.spells[15].RapidSpellMax = (byte)timingRapid.Value;
        }
        #endregion
    }
}
