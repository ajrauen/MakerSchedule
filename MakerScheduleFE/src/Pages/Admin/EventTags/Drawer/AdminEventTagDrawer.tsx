import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEventTag,
} from "@ms/redux/slices/adminSlice";
import type { EventTag } from "@ms/types/event-tags.types";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";
import { IconButton, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { EventTagDetails } from "@ms/Pages/Admin/EventTags/Drawer/Details/EventTagDetails";
import { getEventTag } from "@ms/api/event-tag.api";

interface AdminEventTagDrawerProps {
  onDrawerClose: () => void;
}

const AdminEventTagDrawer = ({ onDrawerClose }: AdminEventTagDrawerProps) => {
  const { selectedEventTag, adminDrawerOpen } =
    useAppSelector(selectAdminState);

  const dispatch = useAppDispatch();

  const { data: eventTagData } = useQuery({
    queryKey: ["eventTag", selectedEventTag?.id],
    queryFn: async () => {
      return getEventTag(selectedEventTag!.id!);
    },
    enabled: !!selectedEventTag?.id && adminDrawerOpen,
  });

  const detailedEventTag = useMemo(() => {
    if (!eventTagData || selectedEventTag?.meta?.isUpdated)
      return selectedEventTag;

    if (JSON.stringify(eventTagData) === JSON.stringify(selectedEventTag)) {
      return selectedEventTag;
    }

    const updatedEventTag: EventTag = {
      ...selectedEventTag,
      ...eventTagData,
    };

    dispatch(setSelectedEventTag(updatedEventTag));
    return updatedEventTag;
  }, [eventTagData, selectedEventTag, dispatch]);

  const handleIconClose = () => {
    onDrawerClose();
  };

  const handleDetailClose = () => {
    onDrawerClose();
  };

  return (
    <Drawer
      anchor="right"
      open={adminDrawerOpen}
      onClose={onDrawerClose}
      slotProps={{
        paper: {
          sx: {
            width: "min(90vw, 800px)",
            height: "100vh",
          },
        },
      }}
    >
      <div className="flex flex-col h-full w-full">
        <div className="ml-auto absolute right-2 top-2 z-10">
          <IconButton onClick={handleIconClose}>
            <CloseIcon />
          </IconButton>
        </div>

        {detailedEventTag && (
          <div className="flex-1 p-4 pt-12">
            <EventTagDetails onClose={handleDetailClose} />
          </div>
        )}
      </div>
    </Drawer>
  );
};

export { AdminEventTagDrawer };
