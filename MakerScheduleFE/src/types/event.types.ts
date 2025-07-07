type EventType = 1 | 2 | 3;

// Event type constants for better readability - matches backend EventTypeEnum
const EVENT_TYPES = {
  WOODWORKING: 1,
  POTTERY: 2,
  SEWING: 3,
} as const;

interface EventOffering {
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  scheduleStart?: number;
  duration?: number;
  price?: number;
  eventType: EventType;
}

export { type EventOffering, type EventType, EVENT_TYPES };
