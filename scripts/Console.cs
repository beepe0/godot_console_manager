using System;
using Godot;

public static class Console 
{
    public static string SetColor(object text, Color color) {
		return ConsoleWindow.Instance?.SetColor(text, color);
	}

	public static string SetColor(object text, string hex) {
		return ConsoleWindow.Instance?.SetColor(text, hex);
	}

	public static void Debug(object text) {
        ConsoleWindow.Instance?.Debug(text);
	}

	public static void Log(object text) {
        ConsoleWindow.Instance?.Debug($"{SetColor($"[{DateTime.Now.ToLongTimeString()}]:", "#21799e")} {text}");
	}

	public static void Warning(object text) {
		ConsoleWindow.Instance?.Debug($"{SetColor($"[{DateTime.Now.ToLongTimeString()}]:", "#eb8334")} {text}");
	}

	public static void Error(object text) {
		ConsoleWindow.Instance?.Debug($"{SetColor($"[{DateTime.Now.ToLongTimeString()}]:", "#963636")} {text}");
	}

	public static void Clear() {
        ConsoleWindow.Instance?.Clear();
	}
} 