using UnityEngine;

public interface IBrailler
{
    void OnIndexWrite(int index);
    void OnBackspaceDown();
    void OnSpaceDown();
}
