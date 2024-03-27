
using NAudio.Wave;
class Sound
{
    public WaveOutEvent device { get; set; }
    public MediaFoundationReader reader { get; set; }
    public string path { get; }
    private long times { get; set; }
    private float volume { get; set; }
    private float currVolume { get; set; }
    private float volumeGradient { get; set; }
    private int keepTime { get; set; }
    private int fadeInTime { get; set; }
    private int fadeOutTime { get; set; }
    private int status { get; set; }

    public Sound(string path)
    {
        device = new WaveOutEvent();
        this.path = path;
        reader = new MediaFoundationReader(path);
        device.Init(reader.ToSampleProvider());

        fadeInTime = 0;
        fadeOutTime = 0;
        keepTime = 0;
        times = 0;

        // If no fadeIn
        status = 1;
        currVolume = 1.0f;

    }

    public void update(long time)
    {
        if (device.PlaybackState == PlaybackState.Playing)
        {

            times += time;
            switch (status)
            {

                case 0:
                    if (times <= fadeInTime)
                    {
                        volumeGradient = gradient(fadeInTime - times);
                        currVolume += volumeGradient;
                        if (currVolume >= volume)
                        {
                            currVolume = volume;
                        }
                        device.Volume = currVolume;


                    }
                    if (Math.Round(currVolume, 1) == volume)
                    {
                        times = 0;
                        status++;
                    }
                    break;
                case 1:
                    Console.WriteLine(times);
                    if (times >= keepTime)
                    {

                        times = 0;
                        status++;
                        volume = 0.0f;
                        if (fadeOutTime == 0)
                        {
                            status = 0;
                            device.Stop();
                            device.Dispose();
                        }
                    }
                    break;
                case 2:
                    if (times <= fadeOutTime)
                    {
                        volumeGradient = gradient(fadeOutTime - times);
                        Console.WriteLine(currVolume);

                        currVolume += volumeGradient;
                        if (currVolume <= 0.0f)
                        {
                            currVolume = 0.0f;
                        }
                        device.Volume = currVolume;

                        if (Math.Round(currVolume, 1) == volume)
                        {
                            times = 0;
                            status = 1;

                            device.Stop();

                            device.Dispose();
                        }
                    }

                    break;
                default:
                    times = 0;
                    break;
            }
        }
    }
    public float gradient(long timeLeft)
    {
        if ((timeLeft) <= 0)
        {
            return (float)(Math.Sign(volume - currVolume) * (Math.Abs(volume - currVolume)));
        }
        else
        {
            return (float)(Math.Sign(volume - currVolume) * (Math.Abs(volume - currVolume) / (timeLeft)));
        }

    }
    public void fadeIn(int fadeInTime)
    {
        if (fadeInTime != 0)
        {
            currVolume = 0.0f;
        }
        else
        {
            status = 1;
        }

        this.fadeInTime = fadeInTime;
    }
    public void fadeOut(int fadeOutTime)
    {
        this.fadeOutTime = fadeOutTime;
    }

    public void Play(int fadeInTime = 0, int keepTime = 0, int fadeOutTime = 0, float volume = 1.0f)
    {

        fadeIn(fadeInTime);
        fadeOut(fadeOutTime);
        this.keepTime = keepTime;
        this.volume = volume;
        this.times = 0;
        device.Volume = currVolume;
        try
        {
            device.Play();
        }
        catch
        {
            //How to play back audio??
            reader = new MediaFoundationReader(path);
            device.Init(reader.ToSampleProvider());
            device.Play();
        }
    }
    public void End()
    {
        reader.Dispose();
    }
    public void Pause()
    {
        device.Pause();
    }
    public void Pause(int fadeOutTime) { }
    public void Close() { }
    public void Close(int fadeOutTime)
    {

        status = 2;
    }

}