export const udpateInput = <T>(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>, current:T, callback: (model:T) => void) => {
    const propValue = (e.target.tagName === 'SELECT' || e.target.type === 'number')  
        ? parseInt(e.target.value)
        : e.target.value;
    const newProp = { [e.target.name]: propValue };
    callback({ ...current, ...newProp });
};