﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LAZYSHELL
{
    [Serializable()]
    public class SPCSound : SPC
    {
        [NonSerialized()]
        private byte[] data;
        private int index;
        private int type;
        public byte delayTime;
        public byte decayFactor;
        public byte echo;
        private List<SPCCommand>[] channels;
        private List<Note>[] notes;
        private bool[] activeChannels;
        //
        public override int Index { get { return index; } set { index = value; } }
        public int Type { get { return type; } set { type = value; } }
        public override byte[] Data { get { return data; } set { data = value; } }
        public override SampleIndex[] Samples { get { return null; } set { } }
        public override List<Percussives> Percussives { get { return null; } set { } }
        public override List<SPCCommand>[] Channels { get { return channels; } set { channels = value; } }
        public override List<Note>[] Notes { get { return notes; } set { notes = value; } }
        public override bool[] ActiveChannels { get { return activeChannels; } set { activeChannels = value; } }
        public override byte DelayTime { get { return delayTime; } set { delayTime = value; } }
        public override byte DecayFactor { get { return decayFactor; } set { decayFactor = value; } }
        public override byte Echo { get { return echo; } set { echo = value; } }
        //
        public SPCSound(int index, int type)
        {
            this.data = Model.Data;
            this.index = index;
            this.type = type;
            //
            int offset;
            if (type == 0)
                offset = index * 4 + 0x042826;
            else
                offset = index * 4 + 0x043E26;
            //
            activeChannels = new bool[2];
            channels = new List<SPCCommand>[2];
            for (int i = 0; i < 2; i++)
            {
                channels[i] = new List<SPCCommand>();
                int soundOffset = Bits.GetShort(Model.Data, offset);
                offset += 2;
                if (soundOffset == 0)
                {
                    activeChannels[i] = false;
                    continue;
                }
                activeChannels[i] = true;
                if (type == 0)
                    soundOffset = soundOffset - 0x3400 + 0x042C26;
                else
                    soundOffset = soundOffset - 0x3400 + 0x044226;
                int length = 0;
                do
                {
                    soundOffset += length;
                    int opcode = data[soundOffset];
                    length = SPCScriptEnums.SPCScriptLengths[opcode];
                    byte[] commandData = Bits.GetByteArray(data, soundOffset, length);
                    channels[i].Add(new SPCCommand(commandData, this, i));
                }
                while (data[soundOffset] != 0xD0 && data[soundOffset] != 0xCD && data[soundOffset] != 0xCE);
            }
        }
        public override void CreateNotes()
        {
        }
        public override void Assemble()
        {
        }
        public void Assemble(ref int offset)
        {
            // first make sure each channel ends with a termination command
            for (int i = 0; i < 2; i++)
            {
                if (channels[i] != null && channels[i].Count > 0)
                {
                    SPCCommand lastSSC = channels[i][channels[i].Count - 1];
                    if (lastSSC.Opcode != 0xD0 && lastSSC.Opcode != 0xCD && lastSSC.Opcode != 0xCE)
                        channels[i].Add(new SPCCommand(new byte[] { 0xD0 }, this, 0));
                }
            }
            //
            int channelSize1 = 0;
            int channelSize2 = 0;
            if (channels[0] != null && activeChannels[0])
                foreach (SPCCommand ssc in channels[0])
                    channelSize1 += ssc.Length;
            if (channels[1] != null && activeChannels[1])
                foreach (SPCCommand ssc in channels[1])
                    channelSize2 += ssc.Length;
            //
            int offsetStart1 = offset;
            int offsetStart2 = offset + channelSize1;
            if (channels[0] == null || !activeChannels[0])
                offsetStart1 = 0;
            if (channels[1] == null || !activeChannels[1])
                offsetStart2 = 0;
            //
            bool channel2in1 = true;
            if (offsetStart1 != 0 && offsetStart2 != 0 && channelSize2 <= channelSize1)
            {
                for (int a = channels[0].Count - 1, b = channels[1].Count - 1; a >= 0 && b >= 0; a--, b--)
                {
                    if (!Bits.Compare(channels[0][a].CommandData, channels[1][b].CommandData))
                        channel2in1 = false;
                }
            }
            else
                channel2in1 = false;
            if (channel2in1)
                offsetStart2 -= channelSize2;
            //
            if (type == 0)
            {
                if (offsetStart1 == 0)
                    Bits.SetShort(data, index * 4 + 0x042826, 0);
                else
                    Bits.SetShort(data, index * 4 + 0x042826, offsetStart1 - 0x042C26 + 0x3400);
                if (offsetStart2 == 0)
                    Bits.SetShort(data, index * 4 + 0x042826 + 2, 0);
                else
                    Bits.SetShort(data, index * 4 + 0x042826 + 2, offsetStart2 - 0x042C26 + 0x3400);
            }
            else
            {
                if (offsetStart1 == 0)
                    Bits.SetShort(data, index * 4 + 0x043E26, 0);
                else
                    Bits.SetShort(data, index * 4 + 0x043E26, offsetStart1 - 0x044226 + 0x3400);
                if (offsetStart2 == 0)
                    Bits.SetShort(data, index * 4 + 0x043E26 + 2, 0);
                else
                    Bits.SetShort(data, index * 4 + 0x043E26 + 2, offsetStart2 - 0x044226 + 0x3400);
            }
            if (channels[0] != null && activeChannels[0])
                foreach (SPCCommand ssc in channels[0])
                {
                    Bits.SetByteArray(data, offsetStart1, ssc.CommandData);
                    offsetStart1 += ssc.Length;
                }
            if (channels[1] != null && activeChannels[1])
                foreach (SPCCommand ssc in channels[1])
                {
                    Bits.SetByteArray(data, offsetStart2, ssc.CommandData);
                    offsetStart2 += ssc.Length;
                }
            if (channel2in1)
                offset += channelSize1;
            else
                offset += channelSize1 + channelSize2;
        }
        public int GetLength()
        {
            int length = 0;
            // first make sure each channel ends with a termination command
            for (int i = 0; i < 2; i++)
            {
                if (channels[i] != null && channels[i].Count > 0)
                {
                    SPCCommand lastSSC = channels[i][channels[i].Count - 1];
                    if (lastSSC.Opcode != 0xD0 && lastSSC.Opcode != 0xCD && lastSSC.Opcode != 0xCE)
                        length++;
                }
            }
            //
            int channelSize1 = 0;
            int channelSize2 = 0;
            if (channels[0] != null && activeChannels[0])
                foreach (SPCCommand ssc in channels[0])
                    channelSize1 += ssc.Length;
            if (channels[1] != null && activeChannels[1])
                foreach (SPCCommand ssc in channels[1])
                    channelSize2 += ssc.Length;
            //
            bool channel2in1 = true;
            if (channels[0] != null && channels[1] != null &&
                activeChannels[0] && activeChannels[1] && 
                channelSize2 <= channelSize1)
            {
                for (int a = channels[0].Count - 1, b = channels[1].Count - 1; a >= 0 && b >= 0; a--, b--)
                {
                    if (!Bits.Compare(channels[0][a].CommandData, channels[1][b].CommandData))
                        channel2in1 = false;
                }
            }
            else
                channel2in1 = false;
            //
            if (channel2in1)
                length += channelSize1;
            else
                length += channelSize1 + channelSize2;
            return length;
        }
        public override void Clear()
        {
            activeChannels = new bool[2];
            channels = new List<SPCCommand>[2];
            channels[0] = new List<SPCCommand>();
            channels[1] = new List<SPCCommand>();
        }
    }
}
