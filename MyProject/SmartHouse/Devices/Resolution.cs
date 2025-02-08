using System;

public class Resolution : ICloneable
{
    public int Height { get; set; }
    public int Width { get; set; }

    public Resolution()
    {
        Height = 600;
        Width = 480;
    }

    public Resolution(int height, int width)
    {
        Height = height;
        Width = width;
    }

    public Resolution(Resolution res)
    {
        Height = res.Height;
        Width = res.Width;
    }

    public Resolution(string resolutionString)
    {
        resolutionString = resolutionString.Substring(resolutionString.IndexOf("(") + 1);
        resolutionString = resolutionString.Substring(0, resolutionString.IndexOf(")"));

        string[] res = resolutionString.Split('x');
        Width = int.Parse(res[0]);
        Height = int.Parse(res[1]);
    }

    public object Clone()
    {
        return new Resolution(this);
    }

    public override string ToString()
    {
        return $"Resolution{{ Height={Height}, Width={Width} }}";
    }

    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;

        Resolution that = (Resolution)obj;
        return Height == that.Height && Width == that.Width;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Height, Width);
    }
}
