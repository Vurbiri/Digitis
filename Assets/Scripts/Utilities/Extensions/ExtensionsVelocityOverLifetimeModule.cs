using static UnityEngine.ParticleSystem;

public static class ExtensionsVelocityOverLifetimeModule
{
    public static MinMaxVector3 LinearToMinMaxVector3(this VelocityOverLifetimeModule self) => new(self.x, self.y, self.z);

    public static void SetLinearFromMinMaxVector3(this ref VelocityOverLifetimeModule self, MinMaxVector3 value)
    {
        self.x = value.X; self.y = value.Y; self.z = value.Z;
    }
}

