interface EventTag {
  id: string;
  name: string;
  createdAt?: string;
  isActive?: boolean;
  meta?: Record<string, string | boolean | number>;
}

interface CreateEventTag {
  name: string;
}

export type { EventTag, CreateEventTag };
