
using NAudio.Wave;
class Sound
{
    public WaveOutEvent device { get; set; }
    public MediaFoundationReader reader { get; set; }

    public string path { get; }
    private long times { get; set; }
    private float volume { get; set; }
    private float currVolume { get; set; }
    public int audioLength { get; set; }

    bool isStopped = false;

    int[] timing = new int[3];

    int status = 0;
    public Sound(string path)
    {

        reader = new MediaFoundationReader(path);
        device = new WaveOutEvent();
        device.Init(reader.ToSampleProvider());

        times = 0;

        //default value
        audioLength = (int)(reader.TotalTime.TotalMilliseconds);
        device.Volume = 0.0f;
    }

    public void update(long time)
    {
        if (device.PlaybackState == PlaybackState.Playing)
        {
            if (status <= 2)
            {
                gradient(timing[status] - times);
                device.Volume = currVolume;
                Console.WriteLine(device.Volume);


            }

            if (times >= timing[status])
            {
                ++status;
                switch (status)
                {
                    case 2:// Setting Parameters for FadeOut
                        volume = 0.0f;
                        break;
                    case 3: // Audio is done, Restarting
                        status = 0;
                        Console.WriteLine(reader.CurrentTime);
                        if (isStopped)
                        {
                            device.Stop();


                        }
                        else
                        {
                            device.Pause();

                        }
                        break;
                }
                times = 0;
            }
            times += time;
        }


    }
    public void Offset(int offset)
    {
        reader.CurrentTime = TimeSpan.FromSeconds(offset);
    }
    public void Pause(int fadeTime = 0)
    {
        times = 0;
        timing[2] = fadeTime;
        status = 2;
        volume = 0.0f;

    }
    public void Stop(int fadeTime = 0)
    {
        isStopped = true;
        Pause(fadeTime);

    }
    public void Play(int fadeInTime = 10, int keepTime = -1, int fadeOutTime = 10, float volume = 1.0f)
    {

        // Error Checking --> audioLength is in milliseconds
        if ((fadeInTime + keepTime + fadeOutTime) > audioLength)
        {
            throw new Exception("Expected fadeInTime + keepTime + fadeOutTime <= audioLength got the opposite.");
        }

        // Setting up parameters
        if (keepTime == -1)
        {
            timing[1] = audioLength - fadeInTime - fadeOutTime;
            Console.WriteLine(timing[1]);

            if (timing[1] < 0)
            {
                throw new Exception("keepTime value is smaller than 0, adjust fadeOutTime and fadeInTime settings");
            }
        }
        else
        {
            timing[1] = keepTime;
        }
        timing[0] = fadeInTime;
        timing[2] = fadeOutTime;

        this.volume = volume;
        if (device.PlaybackState == PlaybackState.Stopped)
        {
            reader.CurrentTime = TimeSpan.Zero;
        }
        status = 0;
        device.Volume = 0.0f;
        // If we are resuming --> want to restart from the current time not 0
        device.Play();
    }
    public void gradient(long timeLeft)
    {
        if (timeLeft <= 0)
        {
            currVolume += (float)(Math.Sign(volume - currVolume) * (Math.Abs(volume - currVolume)));
        }
        else
        {
            currVolume += (float)(Math.Sign(volume - currVolume) * (Math.Abs(volume - currVolume) / (timeLeft)));
        }
    }
}