import { Dialog, DialogTitle, Input, Button } from '@mui/material';
import React, { useState } from 'react'

import { useApi } from '../../contexts/ApiContext';
import { useSnackbar } from 'notistack';

import './AddBalanceDialog.css';

type Props = {
    open: boolean;
    onClose: () => void;
};

function AddBalanceDialog({ open, onClose }: Props) {

    const api = useApi();

    const { enqueueSnackbar } = useSnackbar();

    const [amount, setAmount] = useState(0);

    const handleAmountChange = (e) => {
        setAmount(e.target.value);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const { data: user } = await api.currentUser.addUserBalance({ addUserBalanceRequest: { amount: amount } })
            onClose();

            setAmount(0);

            api.user!.balance = user.balance
        } catch (e) {
            enqueueSnackbar(e.response.data.Message, {
                variant: "error"
            });
        }

    };

    return (
        <Dialog onClose={onClose} open={open}>

            <DialogTitle>Add Balance</DialogTitle>

            <form className='balance-form'>

                <Input
                    value={amount}
                    onChange={handleAmountChange}
                    placeholder='Enter amount...'
                    type='number'
                />

                <Button type='submit' onClick={handleSubmit}>Ok</Button>
            </form>
        </Dialog>
    )
}

export default AddBalanceDialog;