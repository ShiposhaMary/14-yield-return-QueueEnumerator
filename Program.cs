using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueEnumerator
{
    
    public class Queue<T> : IEnumerable<T>
    {
        public  IEnumerator<T> GetEnumerator()
        {
            return new QueueEnumerator(this); //this означает тот объект, из 
                                                 // которого вызывается 
                                                 // GetEnumerator-здесь это 
                                                 //QueueEnumerator.
        }

        //Не будем останавливаться на этом. Просто всегда пишите так.
        //Это связано с архитектурными особенностями IEnumerable<T>,
        //И требует следующего уровня понимания архитектур
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        QueueItem<T> Head;
        QueueItem<T> tail;

        public bool IsEmpty { get { return Head == null; } }

        public void Enqueue(T value)
        {
            if (IsEmpty)
                tail = Head = new QueueItem<T> { Value = value, Next = null };
            else
            {
                var item = new QueueItem<T> { Value = value, Next = null };
                tail.Next = item;
                tail = item;
            }
        }

        public T Dequeue()
        {
            if (Head == null) throw new InvalidOperationException();
            var result = Head.Value;
            Head = Head.Next;
            if (Head == null)
                tail = null;
            return result;
        }
        public class QueueEnumerator: IEnumerator<T>
        {
            Queue<T> queue;
            QueueItem<T> currentItem;

            public QueueEnumerator(Queue<T> queue)
            {
                this.queue = queue;
                //В самом начале перечислитель не смотрит ни на какой элемент
                //Чтобы посмотреть на первый элемент, его нужно сначала переместить.
                this.currentItem = null;
            }

            public T Current
            {
                get { return currentItem.Value; }
            }

            public bool MoveNext()
            {
                //Если мы еще никуда не смотрим - посмотреть на голову очереди
                if (currentItem == null)
                    currentItem = queue.Head;
                //В противном случае, посмотреть на следующий элемент
                else
                    currentItem = currentItem.Next;
                //Вернуть true, если нам удалось перейти на следующий элемент, 
                //или false, если не удалось и из foreach нужно выйти
                return currentItem != null;
            }

            // Остальные методы нас не волнуют. Просто всегда пишите так.
            public void Dispose()
            {

            }
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            public void Reset()
            {

            }
        }
    }

    public class QueueItem<T>
    {
        public T Value { get; set; }
        public QueueItem<T> Next { get; set; }
    }
    class Program
    {
        static void Main()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            foreach (var value in queue)
                Console.WriteLine(value);
        }
    }
}
