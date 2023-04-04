/*
 * Interface is called everytime the game area is clicked
 */

public interface IGadget
{
    bool GadgetUse( PlayerMovement pFrog, System.Func<IGadget, bool> tDoneCallback );
    bool GadgetCancel();
    string name { get; }
}
