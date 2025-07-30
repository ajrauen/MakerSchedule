import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button,
  type DialogProps,
} from "@mui/material";

interface ConfirmationDialogProps extends DialogProps {
  open: boolean;
  title: string;
  details?: string;
  onCancel: () => void;
  onConfirm: () => void;
  cancelText?: string;
  confirmText?: string;
  children?: React.ReactNode;
}

const ConfirmationDialog = ({
  open,
  title,
  details,
  onCancel,
  onConfirm,
  children,
  cancelText = "Cancel",
  confirmText = "Confirm",
  ...props
}: ConfirmationDialogProps) => {
  return (
    <Dialog
      open={open}
      onClose={onCancel}
      aria-labelledby="confirmation-dialog-title"
      aria-describedby="confirmation-dialog-description"
      fullWidth
      {...props}
    >
      <DialogTitle id="confirmation-dialog-title">{title}</DialogTitle>
      <DialogContent>
        {children ? (
          children
        ) : (
          <DialogContentText id="confirmation-dialog-description">
            {details}
          </DialogContentText>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel}>{cancelText}</Button>
        <Button onClick={onConfirm} variant="contained" autoFocus>
          {confirmText}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export { ConfirmationDialog };
