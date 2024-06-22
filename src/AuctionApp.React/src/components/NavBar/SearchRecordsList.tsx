import { Stack } from '@mui/material';
import { SearchRecordDto } from '../../api/openapi-generated';

import './SearchRecordsList.css';

type SearchRecordsListProps = {
    focused: boolean,
    searchRecords: SearchRecordDto[],
};

function SearchRecordsList({ focused, searchRecords }: SearchRecordsListProps) {

    const recordItem = (record: SearchRecordDto) => {
        return (
            <p
                className='w-full my-2 text-ellipsis overflow-hidden'
                key={record.id}>
                {record.searchQuery}
            </p>
        );
    };

    return (
        <>
            {focused &&
                searchRecords &&
                <Stack className='records-stack'>
                    {searchRecords.map(recordItem)}
                </Stack>}
        </>
    )
}

export default SearchRecordsList;