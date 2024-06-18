import { CircularProgress } from '@mui/material';
import './Loading.css';

export default function Loading() {
    return (
        <div className='loading-body'>
            <CircularProgress className='m-auto' />
        </div>
    );
}