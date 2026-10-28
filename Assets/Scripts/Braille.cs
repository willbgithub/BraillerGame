using System;
using System.Collections.Generic;
using UnityEngine;

public class Braille
{
    static Dictionary<char, int> alphabet = new()
    {
        {'a', 1},
        {'b', 3},
        {'c', 9},
        {'d', 25},
        {'e', 17},
        {'f', 11},
        {'g', 27},
        {'h', 19},
        {'i', 10},
        {'j', 26},
        {'k', 5},
        {'l', 7},
        {'m', 13},
        {'n', 29},
        {'o', 21},
        {'p', 15},
        {'q', 31},
        {'r', 23},
        {'s', 14},
        {'t', 30},
        {'u', 37},
        {'v', 39},
        {'w', 58},
        {'x', 45},
        {'y', 61},
        {'z', 53},

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
