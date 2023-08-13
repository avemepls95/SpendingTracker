﻿namespace SpendingTracker.TelegramBot.Buttons;

public class RecursiveButtonGroup : ButtonGroup
{
    public RecursiveButtonGroup(int id, ButtonsGroupOperation operation, string? text = null)
        : base(id, operation, text)
    {
        Next = this;
    }

}