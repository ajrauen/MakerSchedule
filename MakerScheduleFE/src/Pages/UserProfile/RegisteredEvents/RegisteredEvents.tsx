import { getDomainUserByIdEvents } from "@ms/api/domain-user.api";
import { Accordion } from "@ms/Components/Accordion/Accordion";
import type { DomainUser } from "@ms/types/domain-user.types";
import {
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
} from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";

interface PasswordChangeProps {
  userData?: DomainUser;
}

const RegisteredEvents = ({ userData }: PasswordChangeProps) => {
  const { data: eventData } = useQuery({
    queryKey: ["userEvents", userData?.id],
    queryFn: () => getDomainUserByIdEvents(userData!.id!),
    enabled: !!userData?.id,
  });

  const [upComingEvents, pastEvents] = useMemo(() => {
    if (!eventData || eventData.length === 0) return [[], []];
    const now = new Date();
    const upcoming = eventData.filter((event) => {
      const eventDate = new Date(event.occurrenceStartTime);
      return eventDate >= now;
    });
    const past = eventData.filter((event) => {
      const eventDate = new Date(event.occurrenceStartTime);
      return eventDate < now;
    });
    return [upcoming, past];
  }, [eventData]);

  return (
    <Accordion title="Events" containerClassName="w-full max-w-2xl">
      <div className="flex flex-col gap-4 w-full">
        <div>
          <h3 className="text-lg font-semibold">Upcoming Events</h3>
          <div>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Event Name</TableCell>
                  <TableCell>Date</TableCell>
                  <TableCell></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {upComingEvents?.map((event) => (
                  <TableRow key={event.eventId}>
                    <TableCell>{event.eventName}</TableCell>
                    <TableCell>
                      {new Date(event.occurrenceStartTime).toLocaleString()}
                    </TableCell>
                    <TableCell>
                      <Button>Request Cancelation</Button>
                      <Button>Send Message</Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </div>
        <div>
          <h3 className="text-lg font-semibold">Past Events</h3>
          <div>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Event Name</TableCell>
                  <TableCell>Date</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {pastEvents?.map((event) => (
                  <TableRow key={event.eventId}>
                    <TableCell>{event.eventName}</TableCell>
                    <TableCell>
                      {new Date(event.occurrenceStartTime).toLocaleString()}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </div>
      </div>
    </Accordion>
  );
};

export { RegisteredEvents };
