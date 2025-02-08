using System;

public class SmartCamera : SmartDevice
{
    private Resolution resolution;
    private int fileSize;

    public SmartCamera()
        : base()
    {
        this.resolution = new Resolution(600, 480);
        this.fileSize = 128;
    }

    public SmartCamera(int id, string name, State state, double baseCost, Resolution resolution, int fileSize)
        : base(id, name, state, baseCost)
    {
        this.resolution = new Resolution(resolution);
        this.fileSize = fileSize;
    }

    public SmartCamera(SmartCamera input)
        : base(input)
    {
        this.resolution = new Resolution(input.resolution);
        this.fileSize = input.fileSize;
    }

    public Resolution GetResolution() => new Resolution(this.resolution);
    public void SetResolution(Resolution resolution) => this.resolution = new Resolution(resolution);

    public int GetFileSize() => this.fileSize;
    public void SetFileSize(int kbs) => this.fileSize = kbs;

    public double GetConsumptionPerDay()
    {
        return ((double)(resolution.Width / resolution.Height) + (fileSize + GetBaseCost() / 100));
    }

    public SmartCamera Clone()
    {
        return new SmartCamera(this);
    }

    public override string ToString()
    {
        return $"SmartCamera{{ resolution={resolution}, fileSize={fileSize} }}";
    }

    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        if (!base.Equals(obj)) return false;

        SmartCamera that = (SmartCamera)obj;
        return this.resolution.Equals(that.resolution) && this.fileSize == that.fileSize;
    }
}
