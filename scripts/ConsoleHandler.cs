using Godot;

public partial class ConsoleHandler : Node
{
	private bool _isHided = true;

    public override void _Ready()
    {
		if (_isHided) ConsoleWindow.Instance.Hide(); else ConsoleWindow.Instance.Show();
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("console_show"))
		{
			_isHided = !_isHided;
			if (_isHided) ConsoleWindow.Instance.Hide(); else ConsoleWindow.Instance.Show();
		}
	}
}