import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button,
} from "@mui/material";

interface ConfirmationDialogProps {
  open: boolean;
  title: string;
  details: string;
  onCancel: () => void;
  onConfirm: () => void;
  cancelText?: string;
  confirmText?: string;
}

const ConfirmationDialog = ({
  open,
  title,
  details,
  onCancel,
  onConfirm,
  cancelText = "Cancel",
  confirmText = "Confirm",
}: ConfirmationDialogProps) => {
  return (
    <Dialog
      open={open}
      onClose={onCancel}
      aria-labelledby="confirmation-dialog-title"
      aria-describedby="confirmation-dialog-description"
      maxWidth="sm"
      fullWidth
    >
      <DialogTitle id="confirmation-dialog-title">{title}</DialogTitle>
      <DialogContent>
        <DialogContentText id="confirmation-dialog-description">
          {details}
        </DialogContentText>
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
