import type { EventOffering } from "@ms/types/event.types";
import { Button, Paper } from "@mui/material";

interface ClassCardProps {
  event: EventOffering;
}

const ClassCard = ({ event }: ClassCardProps) => {
  return (
    <Paper className="w-full " elevation={3}>
      <div className="flex lg:flex-row gap-4 bg-gray-100 flex-col w-full">
        <img
          className="h-auto object-fit w-full lg:w-2/4 lg-flex-shrink-0 lg:object-center"
          src="https://www.akc.org/wp-content/uploads/2017/11/Pembroke-Welsh-Corgi-standing-outdoors-in-the-fall.jpg"
          alt="filter"
        />
        <div className="flex flex-col gap-2 p-4 ">
          <div className="flex flex-row gap-4 ">
            <h3 className="text-xl font-bold text-purple-300 flex ">
              {event.eventName}
            </h3>
            <div className="ml-auto">ICON</div>
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
              <Button variant="contained">View Schedule</Button>
            </div>
          </p>
        </div>
      </div>
    </Paper>
  );
};

export { ClassCard };
