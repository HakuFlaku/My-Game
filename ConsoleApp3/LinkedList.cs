using System;
namespace ConsoleApp3
{
    public class LinkedList
    {
        protected Node head, tail;
        private int length;

        public LinkedList()
        {
            length = 0;
            head = tail = null;
        }

        //add the given item in order to this LL, only adds an item if it's of type SpellList, or Ability
        public void newItem(Object item)
        {
            if (head == null)
            {
                head = tail = new Node(item);
            }
            else
            {
                tail.setNext(new Node(item));
                tail = tail.getNext();
            }
            length++;
        }

        public void removeItem(int num)
        {
            if (num == 0)
            {
                head = head.getNext();
            }
            else
            {
                Node item = iterate(num - 1);
                item.setNext(item.getNext().getNext());
            }
            length--;
        }

        public int getLength()
        {
            return length;
        }

        public void emptyList()
        {
            head = null;
        }

        //given an int, go that many steps in the LL, if an item exists at that point, return it, otherwise, return NULL
        public Object getItem(int num)
        {
            Node item = iterate(num);
            if (item != null)
                return item.getData();
            else
                return null;
        }

        private Node iterate(int num)
        {
            Node curr = head;

            for(int i = 0; i < num && curr!=null; i++)
            {
                curr = curr.getNext();
            }

            return curr;
        }

        //standard ToString type of method
        public override String ToString()
        {
            String ret = "";
            Node curr = head;
            String temp = null;

            while (curr != null)
            {
                temp = curr.ToString();
                if (temp != null)
                    ret += temp + "\n";
                curr = curr.getNext();
            }

            return ret;
        }

        public String enumerateToString()
        {
            String ret = "";
            Node curr = head;
            String temp = null;
            int i = 1;

            while (curr != null)
            {
                temp = curr.ToString();
                if (temp != null)
                {
                    ret += i + ". " + temp + "\n";
                    i++;
                }
                curr = curr.getNext();
            }

            return ret;
        }

        public bool isEmpty()
        {
            return head == null;
        }

        //encapsulated class Node
        protected class Node
        {
            private Object data;
            private Node next;

            public Node()
            {
                data = null;
                next = null;
            }

            public Node(Object data)
            {
                this.data = data;
                next = null;
            }

            public Node(Object data, Node next)
            {
                this.data = data;
                this.next = next;
            }

            public Node getNext()
            {
                return next;
            }

            public void setNext(Node next)
            {
                this.next = next;
            }

            public Object getData()
            {
                return data;
            }

            public override String ToString()
            {
                return data.ToString();
            }
        }
    }
}
