using System;

public interface IResettingEndScreenController : IEndScreenController
{
    public Action OnResetButtonClick { get; set; }
}
