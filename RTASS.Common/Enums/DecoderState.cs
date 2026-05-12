namespace RTASS.Common.Enums
{
    public enum DecoderState
    {
        SearchingSync, // Senkronizasyon parolası aranıyor
        ReadingLength, // Mesaj uzunluğu okunuyor (32 bit)
        ReadingMessage // Asıl mesaj içeriği okunuyor
    }
}
