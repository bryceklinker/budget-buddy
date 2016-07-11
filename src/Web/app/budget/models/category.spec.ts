import { 
    Category,
    validateCategory
} from './category';

describe('Category', () => {
    let category: Category;

    beforeEach(() => {
        category = { name: 'Bill' };
    });

    it('should have invalid name', () => {
        category.name = undefined;

        const errors = validateCategory(category);
        expect(errors[0].text).toBe('Name is required.');
    });

    it('should have valid name', () => {
        const errors = validateCategory(category);
        expect(errors.length).toBe(0);
    })
})