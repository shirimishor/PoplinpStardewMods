using System.Collections.Generic;

public class AnimalDictData
{
    public string type;
    public Dictionary<int, FrameOffsetData> offsets;
    public bool isBaby;
    public bool initialized;
    public int fixes;

    public AnimalDictData(string type, bool isBaby, int fixes = 0)
    {
        this.type = type;
        this.isBaby = isBaby;
        this.initialized = false;
        this.offsets = new Dictionary<int, FrameOffsetData>();
        this.fixes = fixes;
    }

    public AnimalDictData(AnimalDictData other)
    {
        this.type = other.type;
        this.isBaby = other.isBaby;
        this.initialized = other.initialized;
        this.offsets = other.offsets;
    }

    public void AddFrame(int n, int x, int y, int direction, int flippedX = 9999, int flippedY = 9999)
    {
        
        if (flippedX >= 9999) flippedX = x;
        if (flippedY >= 9999) flippedY = y;
        this.offsets[n] = new FrameOffsetData { X = x, Y = y, flippedX = flippedX, flippedY = flippedY, direction = direction };
    }

    public void CloneFrame(int newFrame, int clonedFrame) 
    {
        if (!this.offsets.ContainsKey(clonedFrame)) return;
        this.offsets[newFrame] = this.offsets[clonedFrame];

    }

    public void Add4FrameAnimation(int startingFrame, int x, int y, int direction, int flippedX = 9999, int flippedY = 9999)
    {
        this.AddFrame(startingFrame, x, y, direction, flippedX, flippedY);
        this.CloneFrame(startingFrame + 2, startingFrame);
        this.AddFrame(startingFrame + 1, x, y+4, direction, flippedX, flippedY+4);
        this.CloneFrame(startingFrame + 3, startingFrame + 1);
    }

}

