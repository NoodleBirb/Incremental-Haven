
public class GuiObject<type> {

    private string text;
    private type scriptHolder;
    public GuiObject (string text, type scriptHolder) {
        this.text = text;
        this.scriptHolder = scriptHolder;
    }
}