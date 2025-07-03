interface EventOffering {
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  scheduleStart?: number;
  duration?: number;
  price?: number;
}

export { type EventOffering };
