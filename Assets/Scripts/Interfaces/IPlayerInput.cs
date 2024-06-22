public interface IPlayerInput
{
    bool IsJump { get; }
    float Horizontal { get; }
    void CustomUpdate();
}