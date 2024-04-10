using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Command
{
    public uint command;
    private int _parityMask;
    private int _stateMask;
    private int _modeMask;
    private int _optionMask;
    private int _musicMask;

    private byte _parityShift;
    private byte _stateShift;
    private byte _modeShift;
    private byte _optionShift;
    public Command(byte stateBits, byte modeBits, byte optionBits, byte musicBits) {

        uint zero = 0;
        byte temp = (byte)(~zero);
        // Command Bit Length (the +1 is for the parity bit)
        byte length = (byte)(stateBits+modeBits+optionBits+musicBits+1);

        _parityShift = (byte)(length-1);
        _stateShift = (byte)(length - stateBits - 1);
        _modeShift = (byte)(length - stateBits - modeBits-1);
        _optionShift = musicBits;

        _parityMask = (temp >> 7) << _parityShift;
        _stateMask = (temp >> (8 - stateBits)) << _stateShift;
        _modeMask = (temp >> (8 - modeBits)) << _modeShift;
        _optionMask = (temp >> (8 - optionBits)) << _optionShift;
        _musicMask = (temp >> (8 - musicBits));

        command = 0;
    }



}
