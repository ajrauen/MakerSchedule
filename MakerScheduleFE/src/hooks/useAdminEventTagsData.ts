import { useQuery } from "@tanstack/react-query";
import { getEventTags } from "@ms/api/event-tag.api";
import type { EventTag } from "@ms/types/event-tags.types";

const useAdminEventTagsData = () => {
  const {
    data: eventTags,
    isLoading: eventTagsLoading,
    error: eventTagsError,
  } = useQuery<EventTag[]>({
    queryKey: ["eventTags"],
    queryFn: getEventTags,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });

  if (eventTagsError) {
    throw Error("Failed to load event tags data");
  }

  return {
    isLoading: eventTagsLoading,
    eventTags: eventTags ?? [],
  };
};

export { useAdminEventTagsData };
