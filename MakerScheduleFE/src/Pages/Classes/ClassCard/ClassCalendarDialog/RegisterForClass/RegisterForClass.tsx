import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { Button } from "@mui/material";
import { useMemo } from "react";

interface RegisterForClassProps {
  event: EventOffering;
  occurrence?: Occurrence;
  closeRegistration: () => void;
}

const RegisterForClass = ({
  event,
  occurrence,
  closeRegistration,
}: RegisterForClassProps) => {
  const startDate = useMemo(() => {
    return occurrence ? new Date(occurrence.scheduleStart) : null;
  }, [occurrence]);

  return (
    <div className="">
      {/* <h1 className="text-lg font-semibold mb-2">{event.eventName}</h1> */}
      <h4 className="text-md font-medium mb-2">
        {startDate?.toLocaleString()}
      </h4>

      <div>
        <Button variant="outlined" color="primary" onClick={closeRegistration}>
          cancel
        </Button>
        <Button variant="contained" color="primary">
          Register
        </Button>
      </div>
    </div>
  );
};
export { RegisterForClass };
