using Godot;

public partial class ConsoleWindow : Window
{
	private static ConsoleWindow _instance;
	private static CommandHandler _commandHandler;

	public static ConsoleWindow Instance {     
		get { 
            if (_instance == null) 
                GD.PrintErr("ConsoleWindow does not exist!");
            return _instance;
        } 
	}

	[ExportGroup("UI")]
	[Export] private RichTextLabel _richTextLabel;
	[Export] private LineEdit _lineEdit;

	private string _onceText;

	public override void _EnterTree() {
		if (_instance == null) _instance = this;
        else QueueFree(); 
	}

	public override void _Ready() {
		_commandHandler = new CommandHandler();
		_commandHandler.Initialize();

		_lineEdit.Text = string.Empty;
	}

	private void _OnWindowCloseRequested() {
		Visible = !Visible;
	}

	private void _OnWindowInput(InputEvent @event) {
		if (@event.IsActionPressed("console_show"))
			_OnWindowCloseRequested();
		else if (@event.IsActionPressed("ui_text_submit")) 
			Send();
	}

	public string SetColor(object text, Color color) {
		return $"[color={color.ToHtml()}]{text}[/color]";
	}

	public string SetColor(object text, string hex) {
		return $"[color={hex}]{text}[/color]";
	}
	
	public void Debug(object text) {
		_richTextLabel.AppendText(text.ToString() + '\n');
	}

	public void Send() {
		_commandHandler.InvokeCommand(_lineEdit.Text);
		_lineEdit.Text = string.Empty;
	}

	public void Clear() {
		_richTextLabel.Clear();
	}
}