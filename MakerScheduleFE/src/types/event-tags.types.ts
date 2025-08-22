interface EventTag {
  id: string;
  name: string;
  isActive?: boolean;
  eventIds?: string[];
  meta?: Record<string, string | boolean | number>;
}

interface CreateEventTag {
  name: string;
}

export type { EventTag, CreateEventTag };
