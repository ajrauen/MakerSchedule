import * as React from "react";
import Button from "@mui/material/Button";
import Dialog, { type DialogProps } from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";

type FormDialogProps = DialogProps & {
  onSubmit: () => void;
  onClose: () => void;
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
      {title ? <DialogTitle>{title}</DialogTitle> : null}
      <DialogContent sx={{ paddingBottom: 0 }}>
        {description ? (
          <DialogContentText>{description}</DialogContentText>
        ) : null}

        {children}
        <DialogActions>
          <Button onClick={onClose}>{closeText}</Button>
          <Button type="submit" onClick={onSubmit}>
            {submitText}
          </Button>
        </DialogActions>
      </DialogContent>
    </Dialog>
  );
};

export { FormDialog };
