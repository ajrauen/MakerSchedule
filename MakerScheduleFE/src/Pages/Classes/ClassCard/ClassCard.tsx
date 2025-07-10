import type { EventOffering } from "@ms/types/event.types";
import { Button, Paper } from "@mui/material";
import { Construction, Circle, ContentCut } from "@mui/icons-material";
import { EVENT_TYPES } from "@ms/types/event.types";
import { ClassCalendarDialog } from "@ms/Pages/Classes/ClassCard/ClassCalendarDialog/ClassCalendarDialog";
import { useState } from "react";

interface ClassCardProps {
  event: EventOffering;
}

const ClassCard = ({ event }: ClassCardProps) => {
  const [isScheduleCardOpen, setIsScheduleCardOpen] = useState(false);

  const getEventIcon = (eventType: number) => {
    switch (eventType) {
      case EVENT_TYPES.WOODWORKING:
        return <Construction sx={{ fontSize: 32, color: "#8B4513" }} />;
      case EVENT_TYPES.POTTERY:
        return <Circle sx={{ fontSize: 32, color: "#D2691E" }} />;
      case EVENT_TYPES.SEWING:
        return <ContentCut sx={{ fontSize: 32, color: "#FF69B4" }} />;
      default:
        return <Construction sx={{ fontSize: 32, color: "#666" }} />;
    }
  };

  console.log(event);

  return (
    <Paper className="w-full " elevation={3}>
      <div className="flex lg:flex-row gap-4 bg-gray-100 flex-col w-full">
        <img
          className="h-auto object-fit w-full lg:w-2/4 lg-flex-shrink-0 lg:object-center"
          src={event.fileUrl}
          alt="filter"
        />
        <div className="flex flex-col gap-2 p-4 ">
          <div className="flex flex-row gap-4 ">
            <h3 className="text-xl font-bold text-purple-300 flex ">
              {event.eventName}
            </h3>
            <div className="ml-auto">{getEventIcon(event.eventType)}</div>
          </div>
          <p className="line-clamp-3 lg:line-clamp-5 md:line-clamp-3 overflow-hidden">
            {event.description}
          </p>
          <p className="flex flex-row mt-auto">
            Price:{" "}
            {event?.price?.toLocaleString("us-US", {
              style: "currency",
              currency: "USD",
            })}
            <div className="ml-auto">
              <Button
                variant="contained"
                onClick={() => setIsScheduleCardOpen(true)}
              >
                View Schedule
              </Button>
            </div>
          </p>
        </div>
      </div>
      <ClassCalendarDialog
        isOpen={isScheduleCardOpen}
        onClose={() => setIsScheduleCardOpen(false)}
        classEvent={event}
      />
    </Paper>
  );
};

export { ClassCard };
