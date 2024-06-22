import { Button, IconButton, Stack } from '@mui/material';
import { SearchRecordDto } from '../../api/openapi-generated';
import CancelIcon from '@mui/icons-material/Cancel';

import './SearchRecordsList.css';
import { useSnackbar } from 'notistack';
import { useNavigate } from 'react-router-dom';
import { useApi } from '../../contexts/ApiContext';

type SearchRecordsListProps = {
    focused: boolean,
    searchRecords: SearchRecordDto[],
    setSearchRecords: React.Dispatch<React.SetStateAction<SearchRecordDto[]>>,
    setIsComponentVisible: React.Dispatch<React.SetStateAction<boolean>>,
};

function SearchRecordsList({ focused, searchRecords, setSearchRecords, setIsComponentVisible }: SearchRecordsListProps) {

    const api = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const handleRecordClick = (query: string) => {
        setIsComponentVisible(false);

        navigate(`/search?q=${query}`);
    };

    const removeRecord = async (recordId: number | null) => {
        if (recordId) {
            try {
                await api.searchRecords.deleteSearchRecord({ id: recordId });

                setSearchRecords(prevState => {
                    return prevState.filter(r => r.id !== recordId);
                });
            } catch (e) {
                enqueueSnackbar(e, {
                    variant: "error"
                });
            }
        } else {
            setSearchRecords(prevState => {
                return prevState.filter(r => r.id !== recordId);
            });
        }
    };

    const removeAllRecords = async () => {
        try {
            await api.searchRecords.deleteAllSearchRecords();

            setSearchRecords([]);
        } catch (e) {
            enqueueSnackbar(e, {
                variant: "error"
            });
        }
    };

    const recordItem = (record: SearchRecordDto, index: number) => {
        return (
            <div
                className="flex flex-row sm:px-5 px-0"
                key={index}
            >
                <Button
                    className="search-record-button"
                    style={{ justifyContent: "flex-start" }}
                    onClick={() => handleRecordClick(record.searchQuery)}
                >
                    <span className="overflow-hidden text-ellipsis">
                        {record.searchQuery}
                    </span>
                </Button>
                <IconButton
                    className="text-slate-500 sm:px-auto px-0"
                    onClick={() => removeRecord(record?.id)}
                >
                    <CancelIcon />
                </IconButton>
            </div>
        );
    };

    return (
        <>
            {
                focused &&
                searchRecords.length !== 0 &&
                <Stack
                    className='records-stack'
                >
                    {searchRecords.map(recordItem)}
                    <div className="border border-solid my-1" />
                    <Button
                        className="w-fit mx-auto my-2"
                        key="clear-records"
                        onClick={removeAllRecords}
                    >
                        Clear Records
                    </Button>
                </Stack>}
        </>
    )
}

export default SearchRecordsList;