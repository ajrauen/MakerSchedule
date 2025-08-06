import { useRef, useEffect } from "react";

interface UseDebounceProps<T> {
  callback: (searchString: T) => void;
  delay?: number;
  value: T;
  enabled?: boolean;
}

const useDebounce = <T>({
  callback,
  value,
  delay = 350,
  enabled = true,
}: UseDebounceProps<T>) => {
  const timeoutRef = useRef<NodeJS.Timeout | null>(null);

  useEffect(() => {
    if (!callback || !enabled) return;

    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }

    timeoutRef.current = setTimeout(() => {
      callback(value);
    }, delay);
    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, [callback, delay, value, enabled]);
};

export { useDebounce };
