using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Dice : ScriptableObject
{
    public enum Letter
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z 
    }

    public Letter[] faces = new Letter[6];

    public static Letter CharToLetter(char str)
    {
        switch (str) {
            case 'A':
                return Dice.Letter.A;
            case 'B':
                return Dice.Letter.B;
            case 'C':
                return Dice.Letter.C;
                 
            case 'D':
                return Dice.Letter.D;
                 
            case 'E':
                return Dice.Letter.E;
                 
            case 'F':
                return Dice.Letter.F;
            case 'G':
                return Dice.Letter.G;
            case 'H':
                return Dice.Letter.H;

            case 'I':
                return Dice.Letter.I;
                 
            case 'J':
                return Dice.Letter.J;
                 
            case 'K':
                return Dice.Letter.K;
                 
            case 'L':
                return Dice.Letter.L;
                 
            case 'M':
                return Dice.Letter.M;
                 
            case 'N':
                return Dice.Letter.N;
                 
            case 'O':
                return Dice.Letter.O;
                 
            case 'P':
                return Dice.Letter.P;
                 
            case 'Q':
                return Dice.Letter.Q;
                 
            case 'R':
                return Dice.Letter.R;
                 
            case 'S':
                return Dice.Letter.S;
                 
            case 'T':
                return Dice.Letter.T;
                 
            case 'U':
                return Dice.Letter.U;
                 
            case 'V':
                return Dice.Letter.V;
                 
            case 'W':
                return Dice.Letter.W;
                 
            case 'X':
                return Dice.Letter.X;
                 
            case 'Y':
                return Dice.Letter.Y;
                 
            case 'Z':
                return Dice.Letter.Z;        
        }
        return Dice.Letter.Z;
    }
}
