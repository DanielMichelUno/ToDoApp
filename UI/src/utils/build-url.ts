export const buildUrl = (...args: string[]) => {
    return args.join('/');
}

export const Verbs = {
    Get: 'GET',
    Post: 'POST',
    Put: 'PUT',
    Delete: 'DELETE',
}