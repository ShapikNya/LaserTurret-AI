using DevTurret.classes.Cashe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.classes.Buffer
{
    public class FrameBuffer : IDisposable
    {
        private readonly FrameSlot[] _slots;
        public int Length => _slots.Length;

        public FrameBuffer(int count, int width, int height)
        {
            _slots = new FrameSlot[count];
            for (int i = 0; i < count; i++)
                _slots[i] = new FrameSlot(width, height);
        }

        public FrameSlot GetFrameSlot (int index)
        {
            if (index >= _slots.Length) throw new ArgumentOutOfRangeException("index out of range");
            return _slots[index];
        }

        public FrameSlot GetLatestUnread() //yolo
        {
            FrameSlot latest = null;
            long maxTimestamp = -1;

            foreach (var slot in _slots)
            {
                lock (slot.Lock)
                {
                    if (!slot.IsRead && slot.Timestamp > maxTimestamp)
                    {
                        latest = slot;
                        maxTimestamp = slot.Timestamp;
                    }
                }
            }

            if (latest == null) new Exception("Слот не найден");

            latest.IsRead = true;

            return latest;
        }

        public FrameSlot GetSlotForWriting(long currentTime)
        {
            FrameSlot target = null;
            long minTimestamp = long.MaxValue;

            foreach (var slot in _slots)
            {
                lock (slot.Lock)
                {
                    if (slot.IsRead) // Слот свободен для записи
                    {
                        target = slot;
                        break;
                    }

                    // Если все заняты, выбираем самый старый кадр для перезаписи
                    if (slot.Timestamp < minTimestamp)
                    {
                        minTimestamp = slot.Timestamp;
                        target = slot;
                    }
                }
            }

            // Обновляем время записи и снимаем флаг прочтения
            lock (target.Lock)
            {
                target.Timestamp = currentTime;
                target.IsRead = false;
            }

            return target;
        }

        public void Dispose()
        {
            foreach (var slot in _slots)
                slot.Dispose();
        }

    }
}
