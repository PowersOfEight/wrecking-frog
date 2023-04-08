/*
 * Interface is called everytime the game area is clicked
 */

public interface IGadget
{
    // bool GadgetUse( Frog pFrog, System.Func<IGadget, bool> tDoneCallback );
    bool GadgetCancel();
    string name { get; }
}
