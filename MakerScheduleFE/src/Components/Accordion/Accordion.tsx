import MuiAccordion, {
  type AccordionProps as MuiAccordionProps,
} from "@mui/material/Accordion";
import AccordionActions from "@mui/material/AccordionActions";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Button from "@mui/material/Button";

interface AccordionProps extends MuiAccordionProps {
  onCancel?: () => void;
  onSubmit?: () => void;
  title?: string;
  containerClassName?: string;
}

const Accordion = ({
  children,
  title,
  onCancel,
  onSubmit,
  containerClassName,
  ...accordionProps
}: AccordionProps) => {
  return (
    <MuiAccordion className={`${containerClassName}`} {...accordionProps}>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1-content"
        id="panel1-header"
      >
        <span>{title}</span>
      </AccordionSummary>
      <AccordionDetails>{children}</AccordionDetails>
      {(onCancel || onSubmit) && (
        <AccordionActions>
          {onCancel && <Button onClick={onCancel}>Cancel</Button>}
          {onSubmit && (
            <Button variant="contained" color="primary" onClick={onSubmit}>
              Update
            </Button>
          )}
        </AccordionActions>
      )}
    </MuiAccordion>
  );
};

export { Accordion };
