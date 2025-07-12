import * as React from "react";
import Button from "@mui/material/Button";
import Dialog, { type DialogProps } from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";

type FormDialogProps = DialogProps & {
  onSubmit: () => void;
  onClose?: () => void;
  title?: string;
  description?: string;
  children: React.ReactNode;
  open: boolean;
  submitText?: string;
  closeText?: string;
};

const FormDialog = ({
  description,
  onClose,
  onSubmit,
  title,
  children,
  open,
  submitText = "Submit",
  closeText = "Close",
  ...props
}: FormDialogProps) => {
  return (
    <Dialog open={open} {...props}>
      <DialogTitle>{title}</DialogTitle>
      <form onSubmit={onSubmit}>
        {title ? <DialogTitle>{title}</DialogTitle> : null}
        <DialogContent>
          {description ? (
            <DialogContentText>{description}</DialogContentText>
          ) : null}
          {children}
          <DialogActions>
            {onClose && <Button onClick={onClose}>{closeText}</Button>}
            <Button variant="contained" type="submit">
              {submitText}
            </Button>
          </DialogActions>
        </DialogContent>
      </form>
    </Dialog>
  );
};

export { FormDialog };
