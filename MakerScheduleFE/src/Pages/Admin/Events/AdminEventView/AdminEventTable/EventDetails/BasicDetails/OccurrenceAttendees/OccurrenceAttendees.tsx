import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Link,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import EmailIcon from "@mui/icons-material/Email";
import { IconButton } from "@ms/Components/IconButton/IconButton";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useState } from "react";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { Editor } from "@ms/Components/QuillEditor/Editor";

interface OccurrenceAttendeesProps {
  occurrence?: Occurrence;
}

const OccurrenceAttendees = ({ occurrence }: OccurrenceAttendeesProps) => {
  const [emailBody, setEmailBody] = useState("");
  const [isEmailOpen, setIsEmailOpen] = useState(false);

  const handleEmailOpen = () => {
    setIsEmailOpen(true);
  };

  const handleEmailClose = () => {
    setIsEmailOpen(false);
    setEmailBody("");
  };

  const handleEmailSend = () => {
    console.log("Send email to all attendees with body:", emailBody);
  };

  return (
    <>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          // ...other props
        >
          <div className="flex items-center justify-between w-full">
            <div>Attendees</div>
            <IconButton
              className="!pr-5"
              aria-label="email all"
              tooltipProps={{ title: "Email all" }}
              onClick={handleEmailOpen}
            >
              <EmailIcon />
            </IconButton>
          </div>
        </AccordionSummary>
        <AccordionDetails>
          <TableContainer className="overflow-auto h-60">
            <Table stickyHeader aria-label="sticky table">
              <TableHead>
                <TableRow>
                  <TableCell>Name</TableCell>
                  <TableCell>Email</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {occurrence?.attendees?.map((attendee, idx) => (
                  <TableRow key={idx} hover>
                    <TableCell>{attendee.firstName}</TableCell>
                    <TableCell>
                      <Link href={`mailto:${attendee.email}?`}>
                        {attendee.email}
                      </Link>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </AccordionDetails>
      </Accordion>
      <ConfirmationDialog
        open={isEmailOpen}
        title="Confirm Email All"
        onCancel={handleEmailClose}
        onConfirm={handleEmailSend}
        maxWidth="xl"
        fullWidth
        slotProps={{
          paper: { className: "h-full" },
        }}
      >
        <div className=" bg-gray-200 h-[100%]">
          <Editor />
        </div>
      </ConfirmationDialog>
    </>
  );
};

export { OccurrenceAttendees };
