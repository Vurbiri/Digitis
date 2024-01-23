using Cysharp.Threading.Tasks;
using System;

public partial class UtilityJS
{
    private UniTaskCompletionSource<bool> taskEndIsAccelerometer;

    public void OnEndIsAccelerometer(int result) => taskEndIsAccelerometer?.TrySetResult(Convert.ToBoolean(result));
}
