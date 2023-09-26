using System;

public static class EventManager {
    public static event Action PlayButtonSound;
    public static void PlayButtonSound_Invoke() => PlayButtonSound?.Invoke();

    public static event Action MainMenuStart;
    public static void MainMenuStart_Invoke() => MainMenuStart?.Invoke();

    public static event Action<Direction> ArrowClick;
    public static void ArrowClick_Invoke(Direction direction) => ArrowClick?.Invoke(direction);
}
