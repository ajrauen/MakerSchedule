import type { EventTag } from "@ms/types/event-tags.types";
import { AdminEventTagsTable } from "./Table/Table";

import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventTag,
} from "@ms/redux/slices/adminSlice";
import { useAdminEventTagsData } from "@ms/hooks/useAdminEventTagsData";
import { AdminEventTagDrawer } from "@ms/Pages/Admin/EventTags/Drawer/AdminEventTagDrawer";
import { EventTagHeader } from "@ms/Pages/Admin/EventTags/Header/EventTagHeader";

const AdminEventTags = () => {
  const [searchString, setSearchString] = useState("");
  const [filteredEventTags, setFilteredEventTags] = useState<EventTag[]>([]);

  const { eventTags } = useAdminEventTagsData();

  const dispatch = useAppDispatch();

  useEffect(() => {
    if (!eventTags) return;
    setFilteredEventTags(structuredClone(eventTags));
  }, [eventTags]);

  const { selectedEventTag } = useAppSelector(selectAdminState);

  const handleEventTagSelect = (eventTag: EventTag) => {
    dispatch(setSelectedEventTag(eventTag));
    dispatch(setAdminDrawerOpen(true));
  };

  const handleCloseDrawer = () => {
    if (selectedEventTag?.meta?.isNew) {
      dispatch(setSelectedEventTag(undefined));
    }
    dispatch(setAdminDrawerOpen(false));
  };

  const handleDrawerClose = () => {
    handleCloseDrawer();
  };

  const handleEventTagCreate = () => {
    const newEventTag: EventTag = {
      id: "",
      name: "",
      meta: { isNew: true },
    };
    dispatch(setSelectedEventTag(newEventTag));
    dispatch(setAdminDrawerOpen(true));
  };

  const handleSearch = (value: string | undefined) => {
    setSearchString(value || "");
  };

  useEffect(() => {
    const filtered =
      eventTags?.filter((eventTag) => {
        return eventTag.name
          ?.toLowerCase()
          .includes(searchString.toLowerCase());
      }) || [];

    setFilteredEventTags(filtered);
  }, [searchString, eventTags]);

  return (
    <>
      <EventTagHeader
        onSearch={handleSearch}
        onCreateEventTag={handleEventTagCreate}
      />
      <AdminEventTagsTable
        eventTags={filteredEventTags}
        onEdit={handleEventTagSelect}
        selectedEventTag={selectedEventTag}
      />
      <AdminEventTagDrawer onDrawerClose={handleDrawerClose} />
    </>
  );
};

export { AdminEventTags };
