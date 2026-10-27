using System;
using System.Collections.Generic;
using UnityEngine;

public class Braille
{
    static Dictionary<char, int> alphabet = new()
    {
        {'A', 1},
        {'B', 3},
        {'C', 9},
        {'D', 25},
        {'E', 17},
        {'F', 11},
        {'G', 27},
        {'H', 19},
        {'I', 10},
        {'J', 26},
        {'K', 5},
        {'L', 7},
        {'M', 13},
        {'N', 29},
        {'O', 21},
        {'P', 15},
        {'Q', 31},
        {'R', 23},
        {'S', 14},
        {'T', 30},
        {'U', 37},
        {'V', 39},
        {'W', 58},
        {'X', 45},
        {'Y', 61},
        {'Z', 53},

        {'0', 26},
        {'1', 1},
        {'2', 3},
        {'3', 9},
        {'4', 25},
        {'5', 17},
        {'6', 11},
        {'7', 27},
        {'8', 19},
        {'9', 10},

        {',', 2},
        {'.', 50},
        {'?', 38},

        {' ', 0 },
        {'\n', 60},
    };
    public static int GetIndex(char character)
    {
        return alphabet[character];
    }
    public static char GetCharacter(int index)
    {
        foreach (KeyValuePair<char, int> pair in alphabet)
        {
            if (pair.Value == index)
            {
                return pair.Key;
            }
        }
        return '\0';
    }
    public static bool ContainsIndex(int index)
    {
        foreach (KeyValuePair<char, int> pair in alphabet)
        {
            if (pair.Value == index)
            {
                return true;
            }
        }
        return false;
    }
    public static bool ContainsCharacter(char character)
    {
        foreach (KeyValuePair<char, int> pair in alphabet)
        {
            if (pair.Key == character)
            {
                return true;
            }
        }
        return false;
    }
}
